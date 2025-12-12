using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Shop.Channels;
using Channel = Optivem.Testing.Channels.Channel;
using Optivem.EShop.SystemTest.Core;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private readonly SystemDsl _dsl;

    public ShopSmokeTest()
    {
        _dsl = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _dsl.Dispose();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        _dsl.Shop(channel).GoToShop()
            .Execute()
            .ShouldSucceed();
    }
}
