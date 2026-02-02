using Commons.Util;
using Optivem.EShop.SystemTest.Base.V5;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V7.External;

public class ErpSmokeTest : BaseSystemDslTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        (await _app.Erp().GoToErp().Execute()).ShouldSucceed();
    }
}
