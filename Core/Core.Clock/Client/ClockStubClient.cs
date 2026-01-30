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

    public Task<ClockClientResult> CheckHealth()
        => _httpClient.Get(HealthEndpoint).ToResultAsync();

    public Task<ClockClientResult<ExtGetTimeResponse>> GetTime()
        => _httpClient.Get<ExtGetTimeResponse>(TimeEndpoint).ToResultAsync();

    public Task<ClockClientResult> ConfigureGetTime(ExtGetTimeResponse response)
        => _wireMockClient.StubGetAsync(ClockTimeEndpoint, HttpStatus.Ok, response)
            .MapErrorAsync(ExtClockErrorResponse.From)
            .ToResultAsync();
}
