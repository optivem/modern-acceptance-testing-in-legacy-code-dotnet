using Commons.Util;
using Optivem.EShop.SystemTest.Base.V3;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V3.System;

public abstract class ShopBaseSmokeTest : BaseDriverTest
{
    protected abstract Task SetUpShopDriverAsync();

    public override async Task InitializeAsync()
    {
        await SetUpShopDriverAsync();
    }

    [Fact]
    public async Task ShouldBeAbleToGoToShop()
    {
        var result = await _shopDriver!.GoToShop();
        result.ShouldBeSuccess();
    }
}
