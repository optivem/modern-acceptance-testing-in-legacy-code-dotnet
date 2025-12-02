using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.SmokeTests;

/// <summary>
/// Base class for channel-based smoke tests that automatically manages channel context and driver lifecycle.
/// </summary>
public abstract class BaseChannelSmokeTest : IDisposable
{
    protected IShopDriver ShopDriver { get; private set; } = null!;

    protected void SetupChannel(string channel)
    {
        ChannelContext.Set(channel);
        ShopDriver = DriverFactory.CreateShopDriver();
    }

    public void Dispose()
    {
        ShopDriver?.Dispose();
        ChannelContext.Clear();
    }
}
