using Optivem.Testing;
using E2eTests.Providers;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Base;
using Commons.Dsl;

namespace E2eTests;

public class E2eTest : BaseSystemTest
{
    private const string SKU = "SKU";
    private const string ORDER_NUMBER = "ORDER_NUMBER";

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPrice(Channel channel)
    {
        await App.Erp().ReturnsProduct()
            .Sku("ABC")
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).PlaceOrder()
            .OrderNumber("ORDER-1001")
            .Sku("ABC")
            .Quantity(5)
            .Country("US")
            .Execute()
            .ShouldSucceed();

        (await App.Shop(channel).ViewOrder()
            .OrderNumber("ORDER-1001")
            .Execute())
            .ShouldSucceed()
            .SubtotalPrice(100.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("20.00", "5", "100.00")]
    [ChannelInlineData("10.00", "3", "30.00")]
    [ChannelInlineData("15.50", "2", "31.00")]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPriceParameterized(Channel channel, string unitPrice, string quantity, string subtotalPrice)
    {
        await App.Erp().ReturnsProduct()
            .Sku("ABC")
            .UnitPrice(unitPrice)
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).PlaceOrder()
            .OrderNumber("ORDER-1001")
            .Sku("ABC")
            .Quantity(quantity)
            .Country("US")
            .Execute()
            .ShouldSucceed();

        (await App.Shop(channel).ViewOrder()
            .OrderNumber("ORDER-1001")
            .Execute())
            .ShouldSucceed()
            .SubtotalPrice(subtotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithInvalidQuantity(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity("invalid-quantity")
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrder(Channel channel)
    {
        await App.Erp().ReturnsProduct()
            .Sku(SKU)
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        (await App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .Execute())
            .ShouldSucceed()
            .OrderNumber(ORDER_NUMBER)
            .OrderNumberStartsWith("ORD-");

        (await App.Shop(channel).ViewOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute())
            .ShouldSucceed()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .UnitPrice(20.00m)
            .SubtotalPrice(100.00m)
            .Status(OrderStatus.Placed)
            .DiscountRateGreaterThanOrEqualToZero()
            .DiscountAmountGreaterThanOrEqualToZero()
            .SubtotalPriceGreaterThanZero()
            .TaxRateGreaterThanOrEqualToZero()
            .TaxAmountGreaterThanOrEqualToZero()
            .TotalPriceGreaterThanZero();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldCancelOrder(Channel channel)
    {
        await App.Erp().ReturnsProduct()
            .Sku(SKU)
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldSucceed();

        (await App.Shop(channel).ViewOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute())
            .ShouldSucceed()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Status(OrderStatus.Cancelled);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNonExistentSku(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku("NON-EXISTENT-SKU-12345")
            .Quantity(5)
            .Country("US")
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
    public async Task ShouldNotBeAbleToViewNonExistentOrder(Channel channel, string orderNumber, string expectedErrorMessage)
    {
        await App.Shop(channel).ViewOrder()
            .OrderNumber(orderNumber)
            .Execute()
            .ShouldFail()
            .ErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNegativeQuantity(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity("-3")
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithZeroQuantity(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity("0")
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptySku(Channel channel, string sku)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(sku)
            .Quantity(5)
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity(emptyQuantity)
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("3.5")]
    [ChannelInlineData("lala")]
    public async Task ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity(nonIntegerQuantity)
            .Country("US")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity(5)
            .Country(emptyCountry)
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithUnsupportedCountry(Channel channel)
    {
        await App.Erp().ReturnsProduct()
            .Sku(SKU)
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).PlaceOrder()
            .Sku(SKU)
            .Quantity(5)
            .Country("XX")
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country does not exist: XX");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullQuantity(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Quantity(null!)
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullSku(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
            .Sku(null!)
            .Execute()
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullCountry(Channel channel)
    {
        await App.Shop(channel).PlaceOrder()
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
    public async Task ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedMessage)
    {
        await App.Shop(channel).CancelOrder()
            .OrderNumber(orderNumber)
            .Execute()
            .ShouldFail()
            .ErrorMessage(expectedMessage);
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldNotCancelAlreadyCancelledOrder(Channel channel)
    {
        await App.Erp().ReturnsProduct()
            .Sku(SKU)
            .UnitPrice(20.00m)
            .Execute()
            .ShouldSucceed();

        await App.Shop(channel).PlaceOrder()
            .OrderNumber(ORDER_NUMBER)
            .Sku(SKU)
            .Quantity(5)
            .Country("US")
            .Execute()
            .ShouldSucceed();

        // Cancel the order first time - should succeed
        await App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldSucceed();

        // Try to cancel the same order again - should fail
        await App.Shop(channel).CancelOrder()
            .OrderNumber(ORDER_NUMBER)
            .Execute()
            .ShouldFail()
            .ErrorMessage("Order has already been cancelled");
    }
}
