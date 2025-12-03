using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Channels.Library;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Channel = Optivem.EShop.SystemTest.Core.Channels.Library.Channel;

namespace Optivem.EShop.SystemTest.E2eTests
{
    public class E2eTest : IDisposable
    {
        private IShopDriver? _shopDriver;
        private ErpApiDriver _erpApiDriver;
        private TaxApiDriver _taxApiDriver;

        public E2eTest()
        {
            _erpApiDriver = DriverFactory.CreateErpApiDriver();
            _taxApiDriver = DriverFactory.CreateTaxApiDriver();
        }

        public void Dispose()
        {
            _shopDriver?.Dispose();
            _erpApiDriver?.Dispose();
            _taxApiDriver?.Dispose();
            ChannelContext.Clear();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldPlaceOrderAndCalculateOriginalPrice(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "ABC-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "20.00").ShouldBeSuccess();

            var placeOrderResult = _shopDriver.PlaceOrder(sku, "5", "US").ShouldBeSuccess();

            var orderNumber = placeOrderResult.GetValue().OrderNumber;

            orderNumber.ShouldStartWith("ORD-");

            var viewOrderResult = _shopDriver.ViewOrder(orderNumber!).ShouldBeSuccess();

            var viewOrderResponse = viewOrderResult.GetValue();
            viewOrderResponse.OrderNumber.ShouldBe(orderNumber);
            viewOrderResponse.Sku.ShouldBe(sku);
            viewOrderResponse.Quantity.ShouldBe(5);
            viewOrderResponse.Country.ShouldBe("US");
            viewOrderResponse.UnitPrice.ShouldBe(20.00m);
            viewOrderResponse.OriginalPrice.ShouldBe(100.00m);
            viewOrderResponse.Status.ShouldBe(OrderStatus.PLACED);

            var discountRate = viewOrderResponse.DiscountRate;
            var discountAmount = viewOrderResponse.DiscountAmount;
            var subtotalPrice = viewOrderResponse.SubtotalPrice;

            discountRate.ShouldBeGreaterThanOrEqualTo(0m);
            discountAmount.ShouldBeGreaterThanOrEqualTo(0m);
            subtotalPrice.ShouldBeGreaterThan(0m);

            var taxRate = viewOrderResponse.TaxRate;
            var taxAmount = viewOrderResponse.TaxAmount;
            var totalPrice = viewOrderResponse.TotalPrice;

            taxRate.ShouldBeGreaterThanOrEqualTo(0m, "Tax rate should be non-negative");
            taxAmount.ShouldBeGreaterThanOrEqualTo(0m, "Tax amount should be non-negative");
            totalPrice.ShouldBeGreaterThan(0m, "Total price should be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldCancelOrder(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "XYZ-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "50.00").ShouldBeSuccess();

            var placeOrderResult = _shopDriver.PlaceOrder(sku, "2", "US").ShouldBeSuccess();

            var orderNumber = placeOrderResult.GetValue().OrderNumber;
            _shopDriver.CancelOrder(orderNumber!).ShouldBeSuccess();

            var viewOrderResult = _shopDriver.ViewOrder(orderNumber!).ShouldBeSuccess();

            var viewOrderResponse = viewOrderResult.GetValue();
            viewOrderResponse.OrderNumber.ShouldBe(orderNumber);
            viewOrderResponse.Sku.ShouldBe(sku);
            viewOrderResponse.Quantity.ShouldBe(2);
            viewOrderResponse.Country.ShouldBe("US");
            viewOrderResponse.UnitPrice.ShouldBe(50.00m);
            viewOrderResponse.OriginalPrice.ShouldBe(100.00m);
            viewOrderResponse.Status.ShouldBe(OrderStatus.CANCELLED);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNonExistentSku(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("NON-EXISTENT-SKU-12345", "5", "US")
                .ShouldBeFailure("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldNotBeAbleToViewNonExistentOrder(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.ViewOrder("NON-EXISTENT-ORDER-12345")
                .ShouldBeFailure("Order NON-EXISTENT-ORDER-12345 does not exist.");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNegativeQuantity(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "DEF-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "30.00").ShouldBeSuccess();

            _shopDriver.PlaceOrder(sku, "-3", "US")
                .ShouldBeFailure("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithZeroQuantity(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "GHI-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "40.00").ShouldBeSuccess();

            _shopDriver.PlaceOrder(sku, "0", "US")
                .ShouldBeFailure("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptySku(Channel channel, string sku)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder(sku, "5", "US")
                .ShouldBeFailure("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("some-sku", emptyQuantity, "US")
                .ShouldBeFailure("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("3.5")]
        [ChannelInlineData("lala")]
        public void ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("some-sku", nonIntegerQuantity, "US")
                .ShouldBeFailure("Quantity must be an integer");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("")]
        [ChannelInlineData("   ")]
        public void ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("some-sku", "5", emptyCountry)
                .ShouldBeFailure("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithUnsupportedCountry(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "JKL-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "25.00").ShouldBeSuccess();

            _shopDriver.PlaceOrder(sku, "3", "XX")
                .ShouldBeFailure("Country does not exist: XX");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullQuantity(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("some-sku", null, "US")
            .ShouldBeFailure("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullSku(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder(null, "5", "US")
                .ShouldBeFailure("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullCountry(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("some-sku", "5", null)
                .ShouldBeFailure("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldNotCancelNonExistentOrder(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.CancelOrder("NON-EXISTENT-ORDER-99999")
                .ShouldBeFailure("Order NON-EXISTENT-ORDER-99999 does not exist.");
        }

        [Theory]
        [MemberData(nameof(GetNonExistentOrderTestData))]
        public void ShouldNotCancelNonExistentOrderParameterized(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.CancelOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
        }

        public static TheoryData<Channel, string, string> GetNonExistentOrderTestData()
        {
            var data = new TheoryData<Channel, string, string>();
            var channels = new[] { ChannelType.API };
            var testCases = new[]
            {
                ("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist."),
                ("INVALID-ORDER-12345", "Order INVALID-ORDER-12345 does not exist."),
                ("FAKE-ORDER-00000", "Order FAKE-ORDER-00000 does not exist.")
            };

            foreach (var channelType in channels)
            {
                foreach (var (orderNum, message) in testCases)
                {
                    data.Add(new Channel(channelType), orderNum, message);
                }
            }

            return data;
        }

        public static TheoryData<Channel, string, string> ShouldNotCancelNonExistentOrderUsingHelperData()
        {
            return GenerateChannelTestData(
                [ChannelType.API], // API only - to add UI, just add ChannelType.UI to the array
                [
                    ("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist."),
                    ("INVALID-ORDER-12345", "Order INVALID-ORDER-12345 does not exist."),
                    ("FAKE-ORDER-00000", "Order FAKE-ORDER-00000 does not exist.")
                ]
            );
        }

        // Alternative: Using a reusable helper method for channel combinations
        [Theory]
        [MemberData(nameof(ShouldNotCancelNonExistentOrderUsingHelperData))]
        public void ShouldNotCancelNonExistentOrderUsingHelper(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.CancelOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
        }

        // Another example: Viewing non-existent orders using the same helper pattern
        [Theory]
        [MemberData(nameof(ShouldNotBeAbleToViewNonExistentOrderUsingHelperData))]
        public void ShouldNotBeAbleToViewNonExistentOrderUsingHelper(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.ViewOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
        }

        public static TheoryData<Channel, string, string> ShouldNotBeAbleToViewNonExistentOrderUsingHelperData()
        {
            return GenerateChannelTestData(
                [ChannelType.UI, ChannelType.API], // Both channels work for viewing orders
                [
                    ("NON-EXISTENT-ORDER-12345", "Order NON-EXISTENT-ORDER-12345 does not exist."),
                    ("INVALID-ORD-99999", "Order INVALID-ORD-99999 does not exist."),
                    ("MISSING-ORDER-00000", "Order MISSING-ORDER-00000 does not exist.")
                ]
            );
        }

        // Another example: Viewing non-existent orders using the same helper pattern
        [Theory]
        [MemberData(nameof(ShouldNotBeAbleToViewNonExistentOrderUsingHelperData2))]
        public void ShouldNotBeAbleToViewNonExistentOrderUsingHelper2(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.ViewOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
        }

        public static IEnumerable<object[]> ShouldNotBeAbleToViewNonExistentOrderUsingHelperData2()
        {
            return GenerateChannelTestDataEnumerable(
                [ChannelType.UI, ChannelType.API], // Both channels work for viewing orders
                [
                    ("NON-EXISTENT-ORDER-12345", "Order NON-EXISTENT-ORDER-12345 does not exist."),
                    ("INVALID-ORD-99999", "Order INVALID-ORD-99999 does not exist."),
                    ("MISSING-ORDER-00000", "Order MISSING-ORDER-00000 does not exist.")
                ]
            );
        }


        // Reusable helper method for any channel + (string, string) test data combination
        private static TheoryData<Channel, string, string> GenerateChannelTestData(
            string[] channels,
            (string value, string message)[] testCases)
        {
            var data = new TheoryData<Channel, string, string>();

            foreach (var channelType in channels)
            {
                foreach (var (value, message) in testCases)
                {
                    data.Add(new Channel(channelType), value, message);
                }
            }

            return data;
        }

        // IEnumerable version - same functionality but returns IEnumerable<object[]>
        private static IEnumerable<object[]> GenerateChannelTestDataEnumerable(
            string[] channels,
            (string value, string message)[] testCases)
        {
            foreach (var channelType in channels)
            {
                foreach (var (value, message) in testCases)
                {
                    yield return new object[] { new Channel(channelType), value, message };
                }
            }
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldNotCancelAlreadyCancelledOrder(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            var sku = "MNO-" + Guid.NewGuid();
            _erpApiDriver.CreateProduct(sku, "35.00").ShouldBeSuccess();

            var placeOrderResult = _shopDriver.PlaceOrder(sku, "3", "US").ShouldBeSuccess();

            var orderNumber = placeOrderResult.GetValue().OrderNumber;

            // Cancel the order first time - should succeed
            _shopDriver.CancelOrder(orderNumber).ShouldBeSuccess();

            // Try to cancel the same order again - should fail
            _shopDriver.CancelOrder(orderNumber)
                .ShouldBeFailure("Order has already been cancelled");
        }
    }
}
