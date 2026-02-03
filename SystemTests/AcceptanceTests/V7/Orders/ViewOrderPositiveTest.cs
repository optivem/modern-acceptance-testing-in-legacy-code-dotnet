using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

#if false // Entire test file disabled
public class ViewOrderPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToViewOrder(Channel channel)
    {
        await Scenario(channel)
            .Given().Order()
            .When().ViewOrder()
            .Then().ShouldSucceed();
    }
}
#endif
