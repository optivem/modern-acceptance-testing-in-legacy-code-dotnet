using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

public class CancelOrderNegativeTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.API)]
    [ChannelInlineData("NON-EXISTENT-ORDER-99999", "Order NON-EXISTENT-ORDER-99999 does not exist.")]
    [ChannelInlineData("NON-EXISTENT-ORDER-88888", "Order NON-EXISTENT-ORDER-88888 does not exist.")]
    [ChannelInlineData("NON-EXISTENT-ORDER-77777", "Order NON-EXISTENT-ORDER-77777 does not exist.")]
    public async Task ShouldNotCancelNonExistentOrder(Channel channel, string orderNumber, string expectedErrorMessage)
    {
        var then = Scenario(channel)
            .When().CancelOrder().WithOrderNumber(orderNumber)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldNotCancelAlreadyCancelledOrder(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Order().WithStatus(OrderStatus.Cancelled)
            .When().CancelOrder()
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("Order has already been cancelled");
    }

    [Theory(Skip = "TODO: FIX")]
    [ChannelData(ChannelType.API)]
    public async Task CannotCancelOrderWhereOrderNumberIsMissing(Channel channel)
    {
        var then = Scenario(channel)
            .When().CancelOrder().WithOrderNumber(null)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("Order null does not exist.");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task CannotCancelNonExistentOrder(Channel channel)
    {
        var then = Scenario(channel)
            .When().CancelOrder().WithOrderNumber("non-existent-order-12345")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("Order non-existent-order-12345 does not exist.");
    }

    [Theory(Skip = "TODO: FIX")]
    [Time]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("2024-12-31T22:00:00Z")]   // Start of blackout period
    [ChannelInlineData("2026-12-31T22:00:01Z")]   // Just after start
    [ChannelInlineData("2025-12-31T22:15:00Z")]   // Middle of blackout period
    [ChannelInlineData("2028-12-31T22:29:59Z")]   // Just before end
    [ChannelInlineData("2021-12-31T22:30:00Z")]   // End of blackout period
    public async Task CannotCancelAnOrderOn31stDecBetween2200And2230(Channel channel, string timeIso)
    {
        var failBuilder = await Scenario(channel)
            .Given().Clock().WithTime(timeIso)
            .And().Order().WithStatus(OrderStatus.Placed)
            .When().CancelOrder()
            .Then().ShouldFail();
        
        failBuilder.ErrorMessage("Order cancellation is not allowed on December 31st between 22:00 and 23:00");
        
        var orderBuilder = await failBuilder.And().Order();
        await orderBuilder.HasStatus(OrderStatus.Placed);
    }
}