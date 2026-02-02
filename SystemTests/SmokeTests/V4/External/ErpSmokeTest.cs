using Commons.Util;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.External;

public class ErpSmokeTest : BaseConfigurableTest, IAsyncLifetime
{
    private ErpRealDriver? _erpDriver;

    public Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        _erpDriver = new ErpRealDriver(configuration.ErpBaseUrl);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _erpDriver?.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        var result = await _erpDriver!.GoToErp();
        result.ShouldBeSuccess();
    }
}
