using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

public class CancelOrderPositiveTest : BaseAcceptanceTest
{
    [Theory(Skip = "TODO: TIME")]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("2024-12-31T21:59:59Z")]   // 1 second before blackout period starts
    [ChannelInlineData("2024-12-31T22:30:01Z")]   // 1 second after blackout period ends
    [ChannelInlineData("2024-12-31T10:00:00Z")]   // Another time on blackout day but outside blackout period
    [ChannelInlineData("2025-01-01T22:15:00Z")]   // Another day entirely (same time but different day)
    public async Task ShouldBeAbleToCancelOrderOutsideOfBlackoutPeriod31stDecBetween2200And2230(Channel channel, string timeIso)
    {
        await Scenario(channel)
            .Given().Clock().WithTime(timeIso)
            .And().Order().WithStatus(OrderStatus.Placed)
            .When().CancelOrder()
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldHaveCancelledStatusWhenCancelled(Channel channel)
    {
        var successBuilder = await Scenario(channel)
            .Given().Order()
            .When().CancelOrder()
            .Then().ShouldSucceed();
        
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasStatus(OrderStatus.Cancelled);
    }
}