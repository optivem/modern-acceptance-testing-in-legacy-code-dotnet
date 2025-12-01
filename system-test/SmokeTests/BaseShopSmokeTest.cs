using Shouldly;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Xunit;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;

namespace Optivem.EShop.SystemTest.SmokeTests;

public abstract class BaseShopSmokeTest : IDisposable
{
    private IShopDriver _shopDriver;

    protected BaseShopSmokeTest()
    {
        _shopDriver = CreateShopDriver();
    }

    protected abstract IShopDriver CreateShopDriver();

    public void Dispose()
    {
        _shopDriver?.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToShop()
    {
        _shopDriver.GoToShop().ShouldBeSuccess();
    }
}
