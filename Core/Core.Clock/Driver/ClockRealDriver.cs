using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockRealDriver : IClockDriver
{
    public void Dispose()
    {
        // No resources to dispose
    }

    public Task<Result<VoidValue, ClockErrorResponse>> GoToClock()
    {
        var _ = DateTimeOffset.UtcNow;
        return Task.FromResult(Result.Success<ClockErrorResponse>());
    }

    public Task<Result<GetTimeResponse, ClockErrorResponse>> GetTime()
    {
        var currentTime = DateTimeOffset.UtcNow;

        var response = new GetTimeResponse
        {
            Time = currentTime
        };

        return Task.FromResult(Result<GetTimeResponse, ClockErrorResponse>.Success(response));
    }

    public Task<Result<VoidValue, ClockErrorResponse>> ReturnsTime(ReturnsTimeRequest request)
    {
        // No-op for real driver - cannot configure system clock
        return Task.FromResult(Result.Success<ClockErrorResponse>());
    }
}
