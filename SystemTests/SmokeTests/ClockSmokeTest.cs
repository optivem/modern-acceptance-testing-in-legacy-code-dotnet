using Optivem.EShop.SystemTest.Core;
using Optivem.Testing;
using Channel = Optivem.Testing.Channel;
using Optivem.EShop.SystemTest.Base;
using Commons.Dsl;

namespace SmokeTests;

public class ClockSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToClock()
    {
        (await _app.Clock().GoToClock()
            .Execute())
            .ShouldSucceed();
    }

    [Fact]
    public async Task ShouldBeAbleToGetTime()
    {
        (await _app.Clock().GetTime()
            .Execute())
            .ShouldSucceed()
            .TimeIsNotNull();
    }
}
