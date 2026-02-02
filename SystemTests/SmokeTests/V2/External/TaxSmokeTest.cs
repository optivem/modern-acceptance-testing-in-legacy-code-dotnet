using Commons.Util;
using Optivem.EShop.SystemTest.Base.V2;
using Xunit;

namespace SystemTests.SmokeTests.V2.External;

public class TaxSmokeTest : BaseClientTest
{
    public override Task InitializeAsync()
    {
        SetUpExternalClients();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        var result = await _taxClient!.CheckHealth();
        result.ShouldBeSuccess();
    }
}
