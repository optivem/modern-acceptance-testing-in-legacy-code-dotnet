using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockRealDriver : IClockDriver
{
    public Result<VoidValue, ClockErrorResponse> GoToClock()
    {
        // Health check by accessing system time
        var _ = DateTimeOffset.UtcNow;
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }

    public Result<GetTimeResponse, ClockErrorResponse> GetTime()
    {
        var response = new GetTimeResponse
        {
            Time = DateTimeOffset.UtcNow
        };
        return Result<GetTimeResponse, ClockErrorResponse>.Success(response);
    }

    public Result<VoidValue, ClockErrorResponse> ReturnsTime(ReturnsTimeRequest request)
    {
        // No-op for real driver - cannot configure system clock
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }

    public void Dispose()
    {
        // No resources to dispose
    }
}
