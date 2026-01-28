using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockRealDriver : IClockDriver
{
    public void Dispose()
    {
        // No resources to dispose
    }

    public Result<VoidValue, ClockErrorResponse> GoToClock()
    {
        var _ = DateTimeOffset.UtcNow;
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }

    public Result<GetTimeResponse, ClockErrorResponse> GetTime()
    {
        var currentTime = DateTimeOffset.UtcNow;

        var response = new GetTimeResponse
        {
            Time = currentTime
        };

        return Result<GetTimeResponse, ClockErrorResponse>.Success(response);
    }

    public Result<VoidValue, ClockErrorResponse> ReturnsTime(ReturnsTimeRequest request)
    {
        // No-op for real driver - cannot configure system clock
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }
}
