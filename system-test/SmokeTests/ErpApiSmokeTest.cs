using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ErpApiSmokeTest : IDisposable
{
    private readonly ErpApiDriver _erpApiDriver;

    public ErpApiSmokeTest()
    {
        _erpApiDriver = DriverFactory.CreateErpApiDriver();
    }

    [Fact]
    public void ShouldCreateProduct()
    {
        var sku = "TEST-" + Guid.NewGuid();
        _erpApiDriver.CreateProduct(sku, "99.99").ShouldBeSuccess();
    }

    public void Dispose()
    {
        _erpApiDriver?.Dispose();
    }
}
