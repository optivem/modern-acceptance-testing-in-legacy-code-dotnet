using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.Testing;
using Channel = Optivem.Testing.Channel;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests;

public class GherkinSmokeTest : BaseSystemTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public Task ShouldPlaceOrderUsingGherkinStyle(Channel channel)
    {
        return Scenario(channel)
            .Given()
                .Product()
                    .WithSku("GHERKIN-SKU")
                    .WithUnitPrice(25.00m)
            .When()
                .PlaceOrder()
                    .WithOrderNumber("GHERKIN-ORDER-001")
                    .WithSku("GHERKIN-SKU")
                    .WithQuantity(3)
            .Then()
                .Order("GHERKIN-ORDER-001")
                    .HasSku("GHERKIN-SKU")
                    .HasQuantity(3)
                    .HasSubtotalPrice(75.00m)
                    .HasStatus(OrderStatus.Placed);
    }

    // [Theory]
    // [ChannelData(ChannelType.UI, ChannelType.API)]
    // public void ShouldCancelOrderUsingGherkinStyle(Channel channel)
    // {
    //     Scenario(channel)
    //         .Given()
    //             .Product()
    //                 .WithSku("CANCEL-SKU")
    //                 .WithUnitPrice(50.00m)
    //                 .And()
    //         .When()
    //             .PlaceOrder()
    //                 .WithOrderNumber("CANCEL-ORDER-001")
    //                 .WithSku("CANCEL-SKU")
    //                 .WithQuantity(2)
    //         .Then()
    //             .Order("CANCEL-ORDER-001")
    //                 .HasStatus(OrderStatus.Placed);

    //     Scenario(channel)
    //         .Given()
    //         .When()
    //             .CancelOrder()
    //                 .WithOrderNumber("CANCEL-ORDER-001")
    //         .Then()
    //             .Order("CANCEL-ORDER-001")
    //                 .HasStatus(OrderStatus.Cancelled);
    // }
}
