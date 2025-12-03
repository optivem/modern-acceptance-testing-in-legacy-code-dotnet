using Shouldly;
using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
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
    private IShopDriver? shopDriver;

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(Channel channel)
    {
        ChannelContext.Set(channel.Value);
        ShopDriver.GoToShop().ShouldBeSuccess();
    }

    private IShopDriver ShopDriver
    {
        get
        {
            if (shopDriver == null)
            {
                shopDriver = DriverFactory.CreateShopDriver();
            }
            return shopDriver;
        }
    }

    public void Dispose()
    {
        shopDriver?.Dispose();
        ChannelContext.Clear();
    }
}
