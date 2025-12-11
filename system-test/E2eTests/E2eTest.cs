using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Optivem.EShop.SystemTest.Core.Dsl;
using System;
using System.Collections.Generic;
using Channel = Optivem.Testing.Channels.Channel;
using Optivem.EShop.SystemTest.E2eTests.Providers;

namespace Optivem.EShop.SystemTest.E2eTests
{
    public class E2eTest : IDisposable
    {
        private readonly Dsl _dsl;

        private const string SKU = "SKU";
        private const string ORDER_NUMBER = "ORDER_NUMBER";

        public E2eTest()
        {
            _dsl = new Dsl();
        }

        public void Dispose()
        {
            _dsl.Dispose();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldPlaceOrderAndCalculateAllPrices(Channel channel)
        {
            _dsl.Erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice(20.00m)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(5)
                .Country("US")
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .OrderNumberStartsWith("ORD-");

            _dsl.Shop(channel).ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(5)
                .Country("US")
                .UnitPrice(20.00m)
                .OriginalPrice(100.00m)
                .Status(OrderStatus.PLACED)
                .DiscountRateGreaterThanOrEqualToZero()
                .DiscountAmountGreaterThanOrEqualToZero()
                .SubtotalPriceGreaterThanZero()
                .TaxRateGreaterThanOrEqualToZero()
                .TaxAmountGreaterThanOrEqualToZero()
                .TotalPriceGreaterThanZero();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldCancelOrder(Channel channel)
        {
            _dsl.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).ViewOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Status(OrderStatus.CANCELLED);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNonExistentSku(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Sku("NON-EXISTENT-SKU-12345")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
        }

        public static IEnumerable<object[]> ShouldNotBeAbleToViewNonExistentOrderData()
        {
            yield return new object[] { "NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist." };
            yield return new object[] { "NON-EXISTENT-ORDER-88888", "Order NON-EXISTENT-ORDER-88888 does not exist." };
            yield return new object[] { "NON-EXISTENT-ORDER-77777", "Order NON-EXISTENT-ORDER-77777 does not exist." };
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelMemberData(nameof(ShouldNotBeAbleToViewNonExistentOrderData))]
        public void ShouldNotBeAbleToViewNonExistentOrder(Channel channel, string orderNumber, string expectedErrorMessage)
        {
            _dsl.Shop(channel).ViewOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedErrorMessage);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNegativeQuantity(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Quantity("-3")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithZeroQuantity(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Quantity("0")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptySku(Channel channel, string sku)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Sku(sku)
                .Execute()
                .ShouldFail()
                .ErrorMessage("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Quantity(emptyQuantity)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("3.5")]
        [ChannelInlineData("lala")]
        public void ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Quantity(nonIntegerQuantity)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must be an integer");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Country(emptyCountry)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithUnsupportedCountry(Channel channel)
        {
            _dsl.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).PlaceOrder()
                .Sku(SKU)
                .Country("XX")
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country does not exist: XX");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullQuantity(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Quantity(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullSku(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Sku(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullCountry(Channel channel)
        {
            _dsl.Shop(channel).PlaceOrder()
                .Country(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        [ChannelInlineData("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist.")]
        [ChannelInlineData("NON-EXISTENT-ORDER-88888", "Order NON-EXISTENT-ORDER-88888 does not exist.")]
        [ChannelInlineData("NON-EXISTENT-ORDER-77777", "Order NON-EXISTENT-ORDER-77777 does not exist.")]
        public void ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _dsl.Shop(channel).CancelOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedMessage);
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldNotCancelAlreadyCancelledOrder(Channel channel)
        {
            _dsl.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _dsl.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            // Cancel the order first time - should succeed
            _dsl.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            // Try to cancel the same order again - should fail
            _dsl.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Order has already been cancelled");
        }
    }
}
