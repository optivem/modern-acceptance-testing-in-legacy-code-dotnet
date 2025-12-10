using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Dsl;
using Optivem.Testing.Channels;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopSmokeTest : IDisposable
{
    private readonly Dsl _dsl;

    public ShopSmokeTest()
    {
        _dsl = new Dsl();
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
