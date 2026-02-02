using Commons.Util;
using Optivem.EShop.SystemTest.Base.V5;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V7.External;

public class ClockSmokeTest : BaseSystemDslTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToClock()
    {
        (await _app.Clock().GoToClock()
            .Execute())
            .ShouldSucceed();
    }
}
