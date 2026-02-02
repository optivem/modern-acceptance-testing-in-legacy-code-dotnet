using Commons.Util;
using Optivem.EShop.SystemTest.Base.V5;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V5.External;

public class TaxSmokeTest : BaseSystemDslTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        var tax = _app.Tax();
        var result = await tax.GoToTax().Execute();
        result.ShouldSucceed();
    }
}
