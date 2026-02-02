using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Base;
using Commons.Dsl;

namespace SmokeTests.External;

public class TaxSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        (await _app.Tax().GoToTax()
            .Execute())
            .ShouldSucceed();
    }
}
