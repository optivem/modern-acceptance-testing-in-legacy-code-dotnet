using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Base;
using Commons.Dsl;

namespace SmokeTests.External;

public class ErpSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        (await _app.Erp().GoToErp()
            .Execute())
            .ShouldSucceed();
    }
}
