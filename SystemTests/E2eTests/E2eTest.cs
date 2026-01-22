using Optivem.Testing.Channels;
using E2eTests.Providers;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Base;

namespace E2eTests;

public class E2eTest : BaseSystemTest
{
    private const string SKU = "SKU";
    private const string ORDER_NUMBER = "ORDER_NUMBER";

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldPlaceOrderWithCorrectSubtotalPrice(Channel channel)
    {
        App.Erp.CreateProduct()
            .Sku("ABC")
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .OrderNumber("ORDER-1001")
            .Sku("ABC")
            .Quantity(5)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).ViewOrder()
            .OrderNumber("ORDER-1001")
            .Execute()
            .ShouldSucceed()
            .SubtotalPrice(100.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("20.00", "5", "100.00")]
    [ChannelInlineData("10.00", "3", "30.00")]
    [ChannelInlineData("15.50", "2", "31.00")]
    public void ShouldPlaceOrderWithCorrectSubtotalPriceParameterized(Channel channel, string unitPrice, string quantity, string subtotalPrice)
    {
        App.Erp.CreateProduct()
            .Sku("ABC")
            .UnitPrice(unitPrice)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .OrderNumber("ORDER-1001")
            .Sku("ABC")
            .Quantity(quantity)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).ViewOrder()
            .OrderNumber("ORDER-1001")
            .Execute()
            .ShouldSucceed()
            .SubtotalPrice(subtotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldRejectOrderWithInvalidQuantity(Channel channel)
    {
        App.Shop(channel).PlaceOrder()
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
        App.Erp.CreateProduct()
            .Sku(SKU)
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .Execute()
            .ShouldSucceed()
            .OrderNumber(ORDER_NUMBER)
            .OrderNumberStartsWith("ORD-");

        App.Shop(channel).ViewOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldSucceed()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .UnitPrice(20.00m)
            .SubtotalPrice(100.00m)
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
        App.Erp.CreateProduct()
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).ViewOrder()
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
        App.Shop(channel).PlaceOrder()
            .Sku("NON-EXISTENT-SKU-12345")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "Product does not exist for SKU: NON-EXISTENT-SKU-12345");
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
        App.Shop(channel).ViewOrder()
            .OrderNumber(orderNumber)
            .Execute()
            .ShouldFail()
            .ErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldRejectOrderWithNegativeQuantity(Channel channel)
    {
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Erp.CreateProduct()
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Country("XX")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country does not exist: XX");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public void ShouldRejectOrderWithNullQuantity(Channel channel)
    {
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).PlaceOrder()
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
        App.Shop(channel).CancelOrder()
            .OrderNumber(orderNumber)
            .Execute()
            .ShouldFail()
            .ErrorMessage(expectedMessage);
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public void ShouldNotCancelAlreadyCancelledOrder(Channel channel)
    {
        App.Erp.CreateProduct()
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        // Cancel the order first time - should succeed
        App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldSucceed();

        // Try to cancel the same order again - should fail
        App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldFail()
            .ErrorMessage("Order has already been cancelled");
    }
}
