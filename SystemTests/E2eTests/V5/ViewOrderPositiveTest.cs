using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.E2eTests.Commons.Constants;
using Optivem.EShop.SystemTest.E2eTests.V5.Base;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V5;

public class ViewOrderPositiveTest : BaseE2eTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldViewPlacedOrder(Channel channel)
    {
        (await _app.Erp().ReturnsProduct().Sku(Defaults.SKU).UnitPrice(25.00m).Execute())
            .ShouldSucceed();

        var shop = await _app.Shop(channel);
        (await shop.PlaceOrder().OrderNumber(Defaults.ORDER_NUMBER).Sku(Defaults.SKU).Country(Defaults.COUNTRY).Quantity(4).Execute())
            .ShouldSucceed();

        (await shop.ViewOrder().OrderNumber(Defaults.ORDER_NUMBER).Execute())
            .ShouldSucceed()
            .OrderNumber(Defaults.ORDER_NUMBER)
            .Sku(Defaults.SKU)
            .Country(Defaults.COUNTRY)
            .Quantity(4)
            .UnitPrice(25.00m)
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
