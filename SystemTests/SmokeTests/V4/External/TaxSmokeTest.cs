using Commons.Util;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.External;

public class TaxSmokeTest : BaseConfigurableTest, IAsyncLifetime
{
    private TaxRealDriver? _taxDriver;

    public Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        _taxDriver = new TaxRealDriver(configuration.TaxBaseUrl);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _taxDriver?.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        var result = await _taxDriver!.GoToTax();
        result.ShouldBeSuccess();
    }
}
