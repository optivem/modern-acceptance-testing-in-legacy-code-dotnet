using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Shop.Channels;
using Channel = Optivem.Testing.Channels.Channel;
using TestDsl = global::Optivem.EShop.SystemTest.Core.Dsl;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private readonly TestDsl _dsl;

    public ShopSmokeTest()
    {
        _dsl = new TestDsl();
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
