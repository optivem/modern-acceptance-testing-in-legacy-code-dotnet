using Commons.Util;
using Optivem.EShop.SystemTest.Base.V4;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.System;

public class ShopSmokeTest : BaseChannelDriverTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToGoToShop(Channel channel)
    {
        // TODO: VJ: This should be made common
        ChannelContext.Set(channel.Type);   
        await base.InitializeAsync();

        var result = await _shopDriver!.GoToShop();
        result.ShouldBeSuccess();
    }
}
