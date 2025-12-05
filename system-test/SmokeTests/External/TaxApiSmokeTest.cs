using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxApiSmokeTest : IDisposable
{
    private readonly TaxApiDriver _taxApiDriver;
    
    public TaxApiSmokeTest()
    {
        _taxApiDriver = DriverFactory.CreateTaxApiDriver();
    }
    
    [Fact]
    public void ShouldBeAbleToGoToTax()
    {
        _taxApiDriver.GoToTax().ShouldBeSuccess();
    }

    public void Dispose()
    {
        _taxApiDriver?.Dispose();
    }
}
