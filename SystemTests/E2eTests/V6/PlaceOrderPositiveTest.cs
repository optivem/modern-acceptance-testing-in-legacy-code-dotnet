using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.E2eTests.V6.Base;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V6;

public class PlaceOrderPositiveTest : BaseE2eTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldCalculateSubtotalPrice(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Product().WithUnitPrice("20.00")
            .When().PlaceOrder().WithQuantity(5)
            .Then();

        var successBuilder = await then.ShouldSucceed();
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasSubtotalPrice("100.00");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPrice(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Product().WithUnitPrice("20.00")
            .When().PlaceOrder().WithQuantity(5)
            .Then();

        var successBuilder = await then.ShouldSucceed();
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasSubtotalPrice("100.00");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("20.00", "5", "100.00")]
    [ChannelInlineData("10.00", "3", "30.00")]
    [ChannelInlineData("15.50", "4", "62.00")]
    [ChannelInlineData("99.99", "1", "99.99")]
    public async Task ShouldPlaceOrderWithCorrectSubtotalPriceParameterized(Channel channel, string unitPrice, string quantity, string subtotalPrice)
    {
        var then = Scenario(channel)
            .Given().Product().WithUnitPrice(unitPrice)
            .When().PlaceOrder().WithQuantity(quantity)
            .Then();

        var successBuilder = await then.ShouldSucceed();
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasSubtotalPrice(subtotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrder(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Product().WithUnitPrice("20.00")
            .When().PlaceOrder().WithQuantity(5)
            .Then();

        var successBuilder = await then.ShouldSucceed();
        successBuilder.HasOrderNumberPrefix("ORD-");
        
        var orderBuilder = await successBuilder.And().Order();
        orderBuilder = await orderBuilder.HasQuantity(5);
        orderBuilder = await orderBuilder.HasUnitPrice(20.00m);
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
