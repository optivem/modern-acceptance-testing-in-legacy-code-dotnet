using Optivem.EShop.SystemTest.Core;
using Optivem.Testing.Channels;
using Channel = Optivem.Testing.Channels.Channel;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests;

public class ClockSmokeTest : BaseSystemTest
{
    [Fact]
    public void ShouldBeAbleToGoToClock()
    {
        App.Clock.GoToClock()
            .Execute()
            .ShouldSucceed();
    }

    [Fact]
    public void ShouldBeAbleToGetTime()
    {
        App.Clock.GetTime()
            .Execute()
            .ShouldSucceed()
            .TimeIsNotNull();
    }
}
