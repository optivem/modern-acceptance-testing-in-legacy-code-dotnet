using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public interface IClockDriver : IDisposable
{
    Result<VoidValue, ClockErrorResponse> GoToClock();
    Result<GetTimeResponse, ClockErrorResponse> GetTime();
    Result<VoidValue, ClockErrorResponse> ReturnsTime(ReturnsTimeRequest request);
}
