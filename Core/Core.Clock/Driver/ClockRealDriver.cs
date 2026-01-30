using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockRealDriver : IClockDriver
{
    public void Dispose()
    {
        // No resources to dispose
    }

    public Task<ClockResult> GoToClock()
    {
        var _ = DateTimeOffset.UtcNow;
        return Task.FromResult(ClockResult.Success());
    }

    public Task<ClockResult<GetTimeResponse>> GetTime()
    {
        var currentTime = DateTimeOffset.UtcNow;

        var response = new GetTimeResponse
        {
            Time = currentTime
        };

        return Task.FromResult(ClockResult<GetTimeResponse>.Success(response));
    }

    public Task<ClockResult> ReturnsTime(ReturnsTimeRequest request)
    {
        // No-op for real driver - cannot configure system clock
        return Task.FromResult(ClockResult.Success());
    }
}
