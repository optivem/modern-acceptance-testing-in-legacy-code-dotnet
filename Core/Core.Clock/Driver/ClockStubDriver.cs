using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client;
using Commons.Util;
using ClientDtos = Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;

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

    public Task<ClockResult> GoToClock()
        => _client.CheckHealth().ToResultAsync();

    public Task<ClockResult<GetTimeResponse>> GetTime()
        => _client.GetTime().MapAsync(GetTimeResponse.From).ToResultAsync();

    public async Task<ClockResult> ReturnsTime(ReturnsTimeRequest request)
    {
        var extResponse = new ExtGetTimeResponse
        {
            Time = DateTimeOffset.Parse(request.Time!)
        };
        await _client.ConfigureGetTime(extResponse);
        return ClockResult.Success();
    }


}
