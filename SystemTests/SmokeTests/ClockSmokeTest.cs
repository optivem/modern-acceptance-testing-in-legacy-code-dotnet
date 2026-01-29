using Optivem.EShop.SystemTest.Core;
using Optivem.Testing;
using Channel = Optivem.Testing.Channel;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests;

public class ClockSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToClock()
    {
        (await App.Clock().GoToClock()
            .Execute())
            .ShouldSucceed();
    }

    [Fact]
    public async Task ShouldBeAbleToGetTime()
    {
        (await App.Clock().GetTime()
            .Execute())
            .ShouldSucceed()
            .TimeIsNotNull();
    }
}
