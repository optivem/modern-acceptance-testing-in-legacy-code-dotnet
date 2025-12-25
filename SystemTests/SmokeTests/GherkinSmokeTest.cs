using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.Testing.Channels;
using Channel = Optivem.Testing.Channels.Channel;

namespace SmokeTests;

public class GherkinSmokeTest : IDisposable
{
    private readonly SystemDsl _app;
    private readonly ScenarioDsl _scenario;

    public GherkinSmokeTest()
    {
        _app = SystemDslFactory.Create();
        _scenario = new ScenarioDsl(_app);
    }

    public void Dispose()
    {
        _app.Dispose();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldPlaceOrderUsingGherkinStyle(Channel channel)
    {
        _scenario
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
                    .OriginalPrice(75.00m)
                    .Status(OrderStatus.PLACED);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldCancelOrderUsingGherkinStyle(Channel channel)
    {
        _scenario
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

        _scenario
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
