namespace Optivem.EShop.SystemTest.SmokeTests.V3.System;

public class ShopApiSmokeTest : ShopBaseSmokeTest
{
    protected override Task SetUpShopDriverAsync()
    {
        SetUpShopApiDriver();
        return Task.CompletedTask;
    }
}
