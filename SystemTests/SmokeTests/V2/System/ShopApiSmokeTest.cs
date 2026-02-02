using Commons.Util;
using Optivem.EShop.SystemTest.Base.V2;
using Xunit;

namespace SystemTests.SmokeTests.V2.System;

public class ShopApiSmokeTest : BaseClientTest
{
    public override Task InitializeAsync()
    {
        SetUpShopApiClient();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToShop()
    {
        var result = await _shopApiClient!.Health().CheckHealth();
        result.ShouldBeSuccess();
    }
}
