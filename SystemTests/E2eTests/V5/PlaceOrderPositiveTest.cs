using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.E2eTests.Commons.Constants;
using Optivem.EShop.SystemTest.E2eTests.V5.Base;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V5;

public class PlaceOrderPositiveTest : BaseE2eTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPrice(Channel channel)
    {
        (await _app.Erp().ReturnsProduct().Sku(Defaults.SKU).UnitPrice(20.00m).Execute())
            .ShouldSucceed();

        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder().OrderNumber(Defaults.ORDER_NUMBER).Sku(Defaults.SKU).Country(Defaults.COUNTRY).Quantity(5).Execute())
            .ShouldSucceed();

        (await shop.ViewOrder().OrderNumber(Defaults.ORDER_NUMBER).Execute())
            .ShouldSucceed()
            .SubtotalPrice(100.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("20.00", "5", "100.00")]
    [ChannelInlineData("10.00", "3", "30.00")]
    [ChannelInlineData("15.50", "4", "62.00")]
    [ChannelInlineData("99.99", "1", "99.99")]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPriceParameterized(Channel channel, string unitPrice, string quantity, string subtotalPrice)
    {
        (await _app.Erp().ReturnsProduct().Sku(Defaults.SKU).UnitPrice(unitPrice).Execute())
            .ShouldSucceed();

        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder().OrderNumber(Defaults.ORDER_NUMBER).Sku(Defaults.SKU).Country(Defaults.COUNTRY).Quantity(quantity).Execute())
            .ShouldSucceed();

        (await shop.ViewOrder().OrderNumber(Defaults.ORDER_NUMBER).Execute())
            .ShouldSucceed()
            .SubtotalPrice(subtotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrder(Channel channel)
    {
        (await _app.Erp().ReturnsProduct().Sku(Defaults.SKU).UnitPrice(20.00m).Execute())
            .ShouldSucceed();

        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder().OrderNumber(Defaults.ORDER_NUMBER).Sku(Defaults.SKU).Country(Defaults.COUNTRY).Quantity(5).Execute())
            .ShouldSucceed()
            .OrderNumber(Defaults.ORDER_NUMBER)
            .OrderNumberStartsWith("ORD-");

        (await shop.ViewOrder().OrderNumber(Defaults.ORDER_NUMBER).Execute())
            .ShouldSucceed()
            .OrderNumber(Defaults.ORDER_NUMBER)
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(5)
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
}
