using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public interface IClockDriver : IDisposable
{
    Task<Result<VoidValue, ClockErrorResponse>> GoToClock();
    Task<Result<GetTimeResponse, ClockErrorResponse>> GetTime();
    Task<Result<VoidValue, ClockErrorResponse>> ReturnsTime(ReturnsTimeRequest request);
}
