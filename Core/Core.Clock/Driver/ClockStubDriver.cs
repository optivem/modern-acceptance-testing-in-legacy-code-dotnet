using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Lang;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockStubDriver : IClockDriver
{
    private readonly ClockStubClient _client;

    public ClockStubDriver(string baseUrl)
    {
        _client = new ClockStubClient(baseUrl);
    }

    public Result<VoidValue, ClockErrorResponse> GoToClock()
    {
        return _client.CheckHealth();
    }

    public Result<GetTimeResponse, ClockErrorResponse> GetTime()
    {
        return _client.GetTime();
    }

    public Result<VoidValue, ClockErrorResponse> ReturnsTime(ReturnsTimeRequest request)
    {
        var response = new GetTimeResponse
        {
            Time = request.Time
        };
        _client.ConfigureGetTime(response);
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}
