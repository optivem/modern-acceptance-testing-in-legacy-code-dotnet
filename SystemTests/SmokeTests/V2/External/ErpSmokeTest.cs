using Commons.Util;
using Optivem.EShop.SystemTest.Base.V2;
using Xunit;

namespace SystemTests.SmokeTests.V2.External;

public class ErpSmokeTest : BaseClientTest
{
    public override Task InitializeAsync()
    {
        SetUpExternalClients();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        var result = await _erpClient!.CheckHealth();
        result.ShouldBeSuccess();
    }
}
