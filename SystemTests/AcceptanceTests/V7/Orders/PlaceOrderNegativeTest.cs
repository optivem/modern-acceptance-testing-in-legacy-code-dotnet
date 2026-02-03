using Optivem.EShop.SystemTest.AcceptanceTests.Commons.Providers;
using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

#if false // Entire test file disabled
public class PlaceOrderNegativeTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithInvalidQuantity(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity("invalid-quantity")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNonExistentSku(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithSku("NON-EXISTENT-SKU-12345")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "Product does not exist for SKU: NON-EXISTENT-SKU-12345");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNegativeQuantity(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity(-10)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithZeroQuantity(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity(0)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptySku(Channel channel, string sku)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithSku(sku)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity(emptyQuantity)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("3.5")]
    [ChannelInlineData("lala")]
    public async Task ShouldRejectOrderWithNonIntegerQuantity(Channel channel, string nonIntegerQuantity)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity(nonIntegerQuantity)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithCountry(emptyCountry)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithUnsupportedCountry(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithCountry("XX")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country does not exist: XX");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullQuantity(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithQuantity(null)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullSku(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithSku(null)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullCountry(Channel channel)
    {
        var then = Scenario(channel)
            .When().PlaceOrder().WithCountry(null)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country must not be empty");
    }
}
#endif
