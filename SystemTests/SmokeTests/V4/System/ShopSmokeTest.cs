using Commons.Util;
using Optivem.EShop.SystemTest.Base.V4;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.System;

public class ShopSmokeTest : BaseChannelDriverTest
{
    // Override to prevent automatic initialization - we need to set ChannelContext first
    public override Task InitializeAsync() => Task.CompletedTask;

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToGoToShop(Channel channel)
    {
        // Set channel context before initializing drivers
        ChannelContext.Set(channel.Type);
        
        // Now initialize with the channel context set
        await base.InitializeAsync();

        try
        {
            var result = await _shopDriver!.GoToShop();
            result.ShouldBeSuccess();
        }
        finally
        {
            await DisposeAsync();
        }
    }
}
