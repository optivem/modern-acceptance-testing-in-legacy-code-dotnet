using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;
using Commons.Http;
using Commons.Util;
using Commons.WireMock;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public class ClockStubClient : IDisposable
{
    private const string HealthEndpoint = "/health";
    private const string TimeEndpoint = "/api/time";
    private const string ClockTimeEndpoint = "/clock/api/time";

    private readonly JsonHttpClient<ExtClockErrorResponse> _httpClient;
    private readonly JsonWireMockClient _wireMockClient;

    public ClockStubClient(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ExtClockErrorResponse>(baseUrl);
        _wireMockClient = new JsonWireMockClient(baseUrl);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _wireMockClient.Dispose();
    }

    public async Task<ExtClockResult> CheckHealth()
    {
        var result = await _httpClient.Get(HealthEndpoint);
        return result.IsSuccess ? ExtClockResult.Success() : ExtClockResult.Failure(result.Error);
    }

    public async Task<ExtClockResult<ExtGetTimeResponse>> GetTime()
    {
        var result = await _httpClient.Get<ExtGetTimeResponse>(TimeEndpoint);
        return result.IsSuccess 
            ? ExtClockResult<ExtGetTimeResponse>.Success(result.Value) 
            : ExtClockResult<ExtGetTimeResponse>.Failure(result.Error);
    }

    public async Task<ExtClockResult> ConfigureGetTime(ExtGetTimeResponse response)
    {
        var result = await _wireMockClient.StubGetAsync(ClockTimeEndpoint, HttpStatus.Ok, response)
            .MapErrorAsync(ExtClockErrorResponse.From);
        return result.IsSuccess ? ExtClockResult.Success() : ExtClockResult.Failure(result.Error);
    }
}
