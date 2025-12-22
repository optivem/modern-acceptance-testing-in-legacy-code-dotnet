using Optivem.Testing.Channels;
using Channel = Optivem.Testing.Channels.Channel;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop;

namespace SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private readonly SystemDsl _app;

    public ShopSmokeTest()
    {
        _app = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _app.Dispose();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        _app.Shop(channel).GoToShop()
            .Execute()
            .ShouldSucceed();
    }
}
