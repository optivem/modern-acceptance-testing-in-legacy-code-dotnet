using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.Testing.Channels;
using Channel = Optivem.Testing.Channels.Channel;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests;

public class GherkinSmokeTest : BaseSystemTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldPlaceOrderUsingGherkinStyle(Channel channel)
    {
        Scenario
            .Given(channel)
                .Product()
                    .Sku("GHERKIN-SKU")
                    .UnitPrice(25.00m)
                    .And()
            .When()
                .PlaceOrder()
                    .OrderNumber("GHERKIN-ORDER-001")
                    .Sku("GHERKIN-SKU")
                    .Quantity(3)
                    .Execute()
            .Then()
                .Order("GHERKIN-ORDER-001")
                    .Has()
                    .Sku("GHERKIN-SKU")
                    .Quantity(3)
                    .SubtotalPrice(75.00m)
                    .Status(OrderStatus.PLACED);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldCancelOrderUsingGherkinStyle(Channel channel)
    {
        Scenario
            .Given(channel)
                .Product()
                    .Sku("CANCEL-SKU")
                    .UnitPrice(50.00m)
                    .And()
            .When()
                .PlaceOrder()
                    .OrderNumber("CANCEL-ORDER-001")
                    .Sku("CANCEL-SKU")
                    .Quantity(2)
                    .Execute()
            .Then()
                .Order("CANCEL-ORDER-001")
                    .Status(OrderStatus.PLACED);

        Scenario
            .Given(channel)
            .When()
                .CancelOrder()
                    .OrderNumber("CANCEL-ORDER-001")
                    .Execute()
            .Then()
                .Order("CANCEL-ORDER-001")
                    .Status(OrderStatus.CANCELLED);
    }
}
