using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Shop;
using Optivem.EShop.SystemTest.Core.Dsl.Erp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Channel = Optivem.Testing.Channels.Channel;

namespace Optivem.EShop.SystemTest.E2eTests
{
    public class E2eTest : IDisposable
    {
        private IShopDriver? _shopDriver;
        private ErpApiDriver _erpApiDriver;
        private TaxApiDriver _taxApiDriver;
        private TestContext _context;
        private ShopDsl? _shop;
        private ErpDsl _erp;

        public E2eTest()
        {
            _erpApiDriver = DriverFactory.CreateErpApiDriver();
            _taxApiDriver = DriverFactory.CreateTaxApiDriver();
            _context = new TestContext();
            _erp = new ErpDsl(_erpApiDriver, _context);
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
            _shop = new ShopDsl(_shopDriver, _context);

            const string SKU = "SKU";
            const string QUANTITY = "5";
            const string COUNTRY = "US";
            const string ORDER_NUMBER = "ORDER_NUMBER";

            _erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice("20.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(SKU)
                .Quantity(QUANTITY)
                .Country(COUNTRY)
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER);

            _shop.ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(5)
                .Country(COUNTRY)
                .Status(OrderStatus.PLACED)
                .SubtotalPriceGreaterThanZero()
                .TaxRateGreaterThanOrEqualToZero()
                .TaxAmountGreaterThanOrEqualToZero()
                .TotalPriceGreaterThanZero();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldCancelOrder(Channel channel)
        {
            _shopDriver = channel.CreateDriver();
            _shop = new ShopDsl(_shopDriver, _context);

            const string SKU = "SKU";
            const string QUANTITY = "2";
            const string COUNTRY = "US";
            const string ORDER_NUMBER = "ORDER_NUMBER";

            _erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice("50.00")
                .Execute()
                .ShouldSucceed();

            _shop.PlaceOrder()
                .Sku(SKU)
                .Quantity(QUANTITY)
                .Country(COUNTRY)
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER);

            _shop.CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            _shop.ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(2)
                .Country(COUNTRY)
                .Status(OrderStatus.CANCELLED);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNonExistentSku(Channel channel)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.PlaceOrder("NON-EXISTENT-SKU-12345", "5", "US")
                .ShouldBeFailure("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
        }

        public static IEnumerable<object[]> ShouldNotBeAbleToViewNonExistentOrderData()
        {
            return GenerateChannelTestDataEnumerable(
                [ChannelType.UI, ChannelType.API],
                [
                    ("NON-EXISTENT-ORDER-12345", "Order NON-EXISTENT-ORDER-12345 does not exist."),
                    ("INVALID-ORD-99999", "Order INVALID-ORD-99999 does not exist."),
                    ("MISSING-ORDER-00000", "Order MISSING-ORDER-00000 does not exist.")
                ]
            );
        }

        [Theory]
        [MemberData(nameof(ShouldNotBeAbleToViewNonExistentOrderData))]
        public void ShouldNotBeAbleToViewNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.ViewOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
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
        [ChannelInlineData("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist.")]
        [ChannelInlineData("INVALID-ORDER-12345", "Order INVALID-ORDER-12345 does not exist.")]
        [ChannelInlineData("FAKE-ORDER-00000", "Order FAKE-ORDER-00000 does not exist.")]
        public void ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _shopDriver = channel.CreateDriver();

            _shopDriver.CancelOrder(orderNumber)
                .ShouldBeFailure(expectedMessage);
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
