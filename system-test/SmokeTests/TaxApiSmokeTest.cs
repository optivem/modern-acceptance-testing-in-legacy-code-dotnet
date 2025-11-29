using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class TaxApiSmokeTest : IDisposable
{
    private readonly TaxApiDriver _taxApiDriver;
    
    public TaxApiSmokeTest()
    {
        _taxApiDriver = DriverFactory.CreateTaxApiDriver();
    }
    
    // TODO: VJ: Add missing smoke tests for Tax API
    
    public void Dispose()
    {
        _taxApiDriver?.Dispose();
    }
}
