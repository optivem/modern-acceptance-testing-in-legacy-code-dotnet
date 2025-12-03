using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // TODO: VJ: Process this

        //[Theory]
        //[InlineData("")]
        //[InlineData("   ")]
        //public void ShouldRejectOrderWithEmptySku(string sku)
        //{
        //    _shopDriver.PlaceOrder(sku, "5", "US")
        //        .ShouldBeFailure("SKU must not be empty");
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("   ")]
        //public void ShouldRejectOrderWithEmptyQuantity(string emptyQuantity)
        //{
        //    _shopDriver.PlaceOrder("some-sku", emptyQuantity, "US")
        //        .ShouldBeFailure("Quantity must not be empty");
        //}

        //[Theory]
        //[InlineData("3.5")]
        //[InlineData("lala")]
        //public void ShouldRejectOrderWithNonIntegerQuantity(string nonIntegerQuantity)
        //{
        //    _shopDriver.PlaceOrder("some-sku", nonIntegerQuantity, "US")
        //        .ShouldBeFailure("Quantity must be an integer");
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("   ")]
        //public void ShouldRejectOrderWithEmptyCountry(string emptyCountry)
        //{
        //    _shopDriver.PlaceOrder("some-sku", "5", emptyCountry)
        //        .ShouldBeFailure("Country must not be empty");
        //}

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
    }
}
