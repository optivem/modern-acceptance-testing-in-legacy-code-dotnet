using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.E2eTests.Providers;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop;

namespace Optivem.EShop.SystemTest.E2eTests
{
    public class E2eTest : IDisposable
    {
        private readonly SystemDsl _app;

        private const string SKU = "SKU";
        private const string ORDER_NUMBER = "ORDER_NUMBER";

        public E2eTest()
        {
            _app = SystemDslFactory.Create();
        }

        public void Dispose()
        {
            _app.Dispose();
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldPlaceOrderWithCorrectOriginalPrice(Channel channel)
        {
            _app.Erp.CreateProduct()
                .Sku("ABC")
                .UnitPrice(20.00m)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
                .OrderNumber("ORDER-1001")
                .Sku("ABC")
                .Quantity(5)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).ViewOrder()
                .OrderNumber("ORDER-1001")
                .Execute()
                .ShouldSucceed()
                .OriginalPrice(100.00m);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("20.00", "5", "100.00")]
        [ChannelInlineData("10.00", "3", "30.00")]
        [ChannelInlineData("15.50", "2", "31.00")]
        public void ShouldPlaceOrderWithCorrectOriginalPriceParameterized(Channel channel, string unitPrice, string quantity, string originalPrice)
        {
            _app.Erp.CreateProduct()
                .Sku("ABC")
                .UnitPrice(unitPrice)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
                .OrderNumber("ORDER-1001")
                .Sku("ABC")
                .Quantity(quantity)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).ViewOrder()
                .OrderNumber("ORDER-1001")
                .Execute()
                .ShouldSucceed()
                .OriginalPrice(originalPrice);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithInvalidQuantity(Channel channel)
        {
            _app.Shop(channel).PlaceOrder()
                .Quantity("invalid-quantity")
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must be an integer");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldPlaceOrder(Channel channel)
        {
            _app.Erp.CreateProduct()
                .Sku(SKU)
                .UnitPrice(20.00m)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Quantity(5)
                .Country("US")
                .Execute()
                .ShouldSucceed()
                .OrderNumber(ORDER_NUMBER)
                .OrderNumberStartsWith("ORD-");

            _app.Shop(channel).ViewOrder()
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
            _app.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).ViewOrder()
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
            _app.Shop(channel).PlaceOrder()
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
            _app.Shop(channel).ViewOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedErrorMessage);
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithNegativeQuantity(Channel channel)
        {
            _app.Shop(channel).PlaceOrder()
                .Quantity("-3")
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithZeroQuantity(Channel channel)
        {
            _app.Shop(channel).PlaceOrder()
                .Quantity("0")
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must be positive");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptySku(Channel channel, string sku)
        {
            _app.Shop(channel).PlaceOrder()
                .Sku(sku)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("sku", "SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
        {
            _app.Shop(channel).PlaceOrder()
                .Quantity(emptyQuantity)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelInlineData("3.5")]
        [ChannelInlineData("lala")]
        public void ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
        {
            _app.Shop(channel).PlaceOrder()
                .Quantity(nonIntegerQuantity)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must be an integer");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        [ChannelClassData(typeof(EmptyArgumentsProvider))]
        public void ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
        {
            _app.Shop(channel).PlaceOrder()
                .Country(emptyCountry)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("country", "Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.UI, ChannelType.API)]
        public void ShouldRejectOrderWithUnsupportedCountry(Channel channel)
        {
            _app.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
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
            _app.Shop(channel).PlaceOrder()
                .Quantity(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("quantity", "Quantity must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullSku(Channel channel)
        {
            _app.Shop(channel).PlaceOrder()
                .Sku(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("sku", "SKU must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldRejectOrderWithNullCountry(Channel channel)
        {
            _app.Shop(channel).PlaceOrder()
                .Country(null!)
                .Execute()
                .ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("country", "Country must not be empty");
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        [ChannelInlineData("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist.")]
        [ChannelInlineData("NON-EXISTENT-ORDER-88888", "Order NON-EXISTENT-ORDER-88888 does not exist.")]
        [ChannelInlineData("NON-EXISTENT-ORDER-77777", "Order NON-EXISTENT-ORDER-77777 does not exist.")]
        public void ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
        {
            _app.Shop(channel).CancelOrder()
                .OrderNumber(orderNumber)
                .Execute()
                .ShouldFail()
                .ErrorMessage(expectedMessage);
        }

        [Theory]
        [ChannelData(ChannelType.API)]
        public void ShouldNotCancelAlreadyCancelledOrder(Channel channel)
        {
            _app.Erp.CreateProduct()
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            _app.Shop(channel).PlaceOrder()
                .OrderNumber(ORDER_NUMBER)
                .Sku(SKU)
                .Execute()
                .ShouldSucceed();

            // Cancel the order first time - should succeed
            _app.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldSucceed();

            // Try to cancel the same order again - should fail
            _app.Shop(channel).CancelOrder()
                .OrderNumber(ORDER_NUMBER)
                .Execute()
                .ShouldFail()
                .ErrorMessage("Order has already been cancelled");
        }
    }
}
