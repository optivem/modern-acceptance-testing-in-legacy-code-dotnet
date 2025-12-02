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
public class ShopSmokeTest
{
    [Theory]
    [ChannelTestData(ChannelType.UI, ChannelType.API)]
    public void ShouldBeAbleToGoToShop(ChannelTestCase channel)
    {
        channel.GetShopDriver().GoToShop().ShouldBeSuccess();
    }
}
