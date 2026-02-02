using Optivem.EShop.SystemTest.Base.V2;
using Shouldly;
using Xunit;

namespace SystemTests.SmokeTests.V2.System;

public class ShopUiSmokeTest : BaseClientTest
{
    public override async Task InitializeAsync()
    {
        await SetUpShopUiClientAsync();
    }

    [Fact]
    public async Task ShouldBeAbleToGoToShop()
    {
        await _shopUiClient!.OpenHomePageAsync();
        (await _shopUiClient.IsPageLoadedAsync()).ShouldBeTrue();
    }
}
