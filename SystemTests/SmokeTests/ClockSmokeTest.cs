using Optivem.EShop.SystemTest.Core;
using Optivem.Testing.Channels;
using Channel = Optivem.Testing.Channels.Channel;

namespace SmokeTests;

public class ClockSmokeTest : IDisposable
{
    private readonly SystemDsl _app;

    public ClockSmokeTest()
    {
        _app = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _app.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToClock()
    {
        _app.Clock.GoToClock()
            .Execute()
            .ShouldSucceed();
    }

    [Fact]
    public void ShouldBeAbleToGetTime()
    {
        _app.Clock.GetTime()
            .Execute()
            .ShouldSucceed()
            .TimeIsNotNull();
    }
}
