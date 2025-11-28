using FluentAssertions;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public abstract class BaseShopSmokeTest : IDisposable
{
    private IShopDriver _shopDriver = null!;

    protected BaseShopSmokeTest()
    {
        _shopDriver = CreateDriver();
    }

    protected abstract IShopDriver CreateDriver();

    public void Dispose()
    {
        Closer.Close(_shopDriver);
    }

    [Fact]
    public void ShouldBeAbleToGoToShop()
    {
        var result = _shopDriver.GoToShop();
        if (!result.Success)
        {
            var errors = result.GetErrors();
            result.Success.Should().BeTrue($"Expected to connect to shop, but got errors: {string.Join(", ", errors)}");
        }
        result.Success.Should().BeTrue();
    }
}
