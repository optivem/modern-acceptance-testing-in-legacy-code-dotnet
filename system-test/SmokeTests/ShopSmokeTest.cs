using Shouldly;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
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
    public void ShouldBeAbleToGoToShop(string channel)
    {
        // Set the channel context for this test execution
        ChannelContext.Set(channel);
        
        try
        {
            // Create driver based on current channel context
            _shopDriver = DriverFactory.CreateShopDriver();
            
            // Execute the test
            _shopDriver.GoToShop().ShouldBeSuccess();
        }
        finally
        {
            // Clean up channel context
            ChannelContext.Clear();
        }
    }

    public void Dispose()
    {
        _shopDriver?.Dispose();
    }
}
