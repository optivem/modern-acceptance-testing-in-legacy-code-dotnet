using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.Testing.Assertions;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpApiSmokeTest : IDisposable
{
    private readonly ErpApiDriver _erpApiDriver;

    public ErpApiSmokeTest()
    {
        _erpApiDriver = DriverFactory.CreateErpApiDriver();
    }

    [Fact]
    public void ShouldBeAbleToGoToErp()
    {
        _erpApiDriver.GoToErp().ShouldBeSuccess();
    }

    public void Dispose()
    {
        _erpApiDriver?.Dispose();
    }
}
