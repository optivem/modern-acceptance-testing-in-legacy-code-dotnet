using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public interface IClockDriver : IDisposable
{
    Task<ClockResult> GoToClock();
    Task<ClockResult<GetTimeResponse>> GetTime();
    Task<ClockResult> ReturnsTime(ReturnsTimeRequest request);
}
