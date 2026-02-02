using Commons.Util;
using Optivem.EShop.SystemTest.Base.V3;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V3.External;

public class TaxSmokeTest : BaseDriverTest
{
    public override Task InitializeAsync()
    {
        SetUpExternalDrivers();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        var result = await _taxDriver!.GoToTax();
        result.ShouldBeSuccess();
    }
}
