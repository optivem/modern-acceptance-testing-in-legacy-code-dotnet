using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.E2eTests.Commons.Constants;
using Optivem.EShop.SystemTest.E2eTests.V6.Base;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V6;

public class ViewOrderPositiveTest : BaseE2eTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldViewPlacedOrder(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Product().WithSku(Defaults.SKU).WithUnitPrice("25.00")
            .And().Order().WithOrderNumber(Defaults.ORDER_NUMBER).WithSku(Defaults.SKU).WithCountry(Defaults.COUNTRY).WithQuantity(4)
            .When().ViewOrder().WithOrderNumber(Defaults.ORDER_NUMBER)
            .Then();

        var successBuilder = await then.ShouldSucceed();
        var orderBuilder = await successBuilder.And().Order(Defaults.ORDER_NUMBER);
        orderBuilder = await orderBuilder.HasSku(Defaults.SKU);
        orderBuilder = await orderBuilder.HasQuantity(4);
        orderBuilder = await orderBuilder.HasCountry(Defaults.COUNTRY);
        orderBuilder = await orderBuilder.HasUnitPrice(25.00m);
        orderBuilder = await orderBuilder.HasSubtotalPrice("100.00");
        orderBuilder = await orderBuilder.HasStatus(OrderStatus.Placed);
        orderBuilder = await orderBuilder.HasDiscountRateGreaterThanOrEqualToZero();
        orderBuilder = await orderBuilder.HasDiscountAmountGreaterThanOrEqualToZero();
        orderBuilder = await orderBuilder.HasSubtotalPriceGreaterThanZero();
        orderBuilder = await orderBuilder.HasTaxRateGreaterThanOrEqualToZero();
        orderBuilder = await orderBuilder.HasTaxAmountGreaterThanOrEqualToZero();
        await orderBuilder.HasTotalPriceGreaterThanZero();
    }
}
