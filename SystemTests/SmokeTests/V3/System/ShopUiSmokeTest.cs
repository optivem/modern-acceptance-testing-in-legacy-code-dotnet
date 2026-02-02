namespace Optivem.EShop.SystemTest.SmokeTests.V3.System;

public class ShopUiSmokeTest : ShopBaseSmokeTest
{
    protected override async Task SetUpShopDriverAsync()
    {
        await SetUpShopUiDriverAsync();
    }
}
