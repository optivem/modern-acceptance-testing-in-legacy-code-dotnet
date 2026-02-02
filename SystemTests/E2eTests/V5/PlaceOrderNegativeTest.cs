using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.E2eTests.Commons.Constants;
using Optivem.EShop.SystemTest.E2eTests.V5.Base;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V5;

public class PlaceOrderNegativeTest : BaseE2eTest
{
    public static IEnumerable<object[]> EmptyValues()
    {
        yield return new object[] { "" };        // Empty string
        yield return new object[] { "   " };     // Whitespace string
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithInvalidQuantity(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity("invalid-quantity")
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNonExistentSku(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku("NON-EXISTENT-SKU-12345")
            .Quantity(Defaults.QUANTITY)
            .Country(Defaults.COUNTRY)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "Product does not exist for SKU: NON-EXISTENT-SKU-12345");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithNegativeQuantity(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(-10)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithZeroQuantity(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku("ANOTHER-SKU-67890")
            .Country(Defaults.COUNTRY)
            .Quantity(0)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelMemberData(nameof(EmptyValues))]
    public async Task ShouldRejectOrderWithEmptySku(Channel channel, string sku)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(sku)
            .Quantity(Defaults.QUANTITY)
            .Country(Defaults.COUNTRY)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelMemberData(nameof(EmptyValues))]
    public async Task ShouldRejectOrderWithEmptyQuantity(Channel channel, string emptyQuantity)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(emptyQuantity)
            .Execute())
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
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(nonIntegerQuantity)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must be an integer");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelMemberData(nameof(EmptyValues))]
    public async Task ShouldRejectOrderWithEmptyCountry(Channel channel, string emptyCountry)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Quantity(Defaults.QUANTITY)
            .Country(emptyCountry)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectOrderWithUnsupportedCountry(Channel channel)
    {
        (await _app.Erp().ReturnsProduct()
            .Sku(Defaults.SKU)
            .Execute())
            .ShouldSucceed();

        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Quantity(Defaults.QUANTITY)
            .Country("XX")
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country does not exist: XX");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullQuantity(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(null)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("quantity", "Quantity must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullSku(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(null)
            .Quantity(Defaults.QUANTITY)
            .Country(Defaults.COUNTRY)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("sku", "SKU must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectOrderWithNullCountry(Channel channel)
    {
        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder()
            .Sku(Defaults.SKU)
            .Quantity(Defaults.QUANTITY)
            .Country(null)
            .Execute())
            .ShouldFail()
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("country", "Country must not be empty");
    }
}
