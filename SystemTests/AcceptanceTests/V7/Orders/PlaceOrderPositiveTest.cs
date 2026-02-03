using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

#if false // Entire test file disabled
public class PlaceOrderPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToPlaceOrderForValidInput(Channel channel)
    {
        await Scenario(channel)
            .Given().Product().WithSku("ABC").WithUnitPrice("20.00")
            .And().Country().WithCode("US").WithTaxRate("0.10")
            .When().PlaceOrder().WithSku("ABC").WithQuantity(5).WithCountry("US")
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task OrderStatusShouldBePlacedAfterPlacingOrder(Channel channel)
    {
        var successBuilder = await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed();

        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasStatus(OrderStatus.Placed);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldCalculateBasePriceAsProductOfUnitPriceAndQuantity(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Product().WithUnitPrice("20.00")
            .When().PlaceOrder().WithQuantity(5)
            .Then().Order();

        await orderBuilder.HasBasePrice("100.00");
    }
}
#endif
