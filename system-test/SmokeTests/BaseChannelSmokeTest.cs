using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.SmokeTests;

/// <summary>
/// Base class for channel-based smoke tests that automatically manages channel context and driver lifecycle.
/// </summary>
public abstract class BaseChannelSmokeTest : IDisposable
{
    private IShopDriver? _shopDriver;
    
    protected IShopDriver ShopDriver 
    { 
        get
        {
            if (_shopDriver == null)
            {
                _shopDriver = DriverFactory.CreateShopDriver();
            }
            return _shopDriver;
        }
    }

    protected void SetupChannel(string channel)
    {
        ChannelContext.Set(channel);
    }

    public void Dispose()
    {
        _shopDriver?.Dispose();
        ChannelContext.Clear();
    }
}
