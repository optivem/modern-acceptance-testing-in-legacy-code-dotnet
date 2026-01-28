using Optivem.Testing;
using Channel = Optivem.Testing.Channel;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests;

public class ShopSmokeTest : BaseSystemTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        App.Shop(channel).GoToShop()
            .Execute()
            .ShouldSucceed();
    }
}
