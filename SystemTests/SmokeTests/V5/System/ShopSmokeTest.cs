using Commons.Util;
using Optivem.EShop.SystemTest.Base.V5;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V5.System;

public class ShopSmokeTest : BaseSystemDslTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToGoToShop(Channel channel)
    {
        (await (await _app.Shop(channel)).GoToShop().Execute()).ShouldSucceed();
    }
}
