using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Xunit;
using static Optivem.EShop.SystemTest.Core.Drivers.Commons.ResultAssert;

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
        var createProductResult = _erpApiDriver.CreateProduct(sku, "99.99");
        AssertThatResult(createProductResult).IsSuccess();
    }

    public void Dispose()
    {
        _erpApiDriver?.Dispose();
    }
}
