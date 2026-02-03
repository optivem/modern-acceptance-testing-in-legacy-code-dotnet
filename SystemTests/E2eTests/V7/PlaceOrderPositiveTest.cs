using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.E2eTests.V7.Base;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests.V7;

public class PlaceOrderPositiveTest : BaseE2eTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPlaceOrder(Channel channel)
    {
        await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed();
    }
}
