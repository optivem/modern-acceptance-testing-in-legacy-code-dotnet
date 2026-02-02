using Commons.Util;
using Optivem.EShop.SystemTest.Base.V5;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V5.External;

public class ErpSmokeTest : BaseSystemDslTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        var erp = _app.Erp();
        var result = await erp.GoToErp().Execute();
        result.ShouldSucceed();
    }
}
