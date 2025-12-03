using Shouldly;
using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

/// <summary>
/// Channel-based smoke test that runs across multiple channels (UI and API).
/// This approach is similar to Java's @Channel annotation pattern.
/// </summary>
public class ShopSmokeTest : IDisposable
{
    private IShopDriver? _shopDriver;

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        _shopDriver = channel.CreateDriver();

        _shopDriver.GoToShop().ShouldBeSuccess();
    }

    public void Dispose()
    {
        _shopDriver?.Dispose();
        ChannelContext.Clear();
    }
}
