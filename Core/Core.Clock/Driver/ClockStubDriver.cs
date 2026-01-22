using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client;
using Optivem.Commons.Util;
using ClientDtos = Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver;

public class ClockStubDriver : IClockDriver
{
    private readonly ClockStubClient _client;

    public ClockStubDriver(string baseUrl)
    {
        _client = new ClockStubClient(baseUrl);
    }
    
    public void Dispose()
    {
        _client?.Dispose();
    }

    public Result<VoidValue, ClockErrorResponse> GoToClock()
    {
        return _client.CheckHealth()
            .MapError(ClockErrorResponse.From);
    }

    public Result<GetTimeResponse, ClockErrorResponse> GetTime()
    {
        return _client.GetTime()
            .Map(GetTimeResponse.From)
            .MapError(ClockErrorResponse.From);
    }

    public Result<VoidValue, ClockErrorResponse> ReturnsTime(ReturnsTimeRequest request)
    {
        var extResponse = new ClientDtos.ExtGetTimeResponse
        {
            Time = request.Time
        };
        _client.ConfigureGetTime(extResponse);
        return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
    }


}
