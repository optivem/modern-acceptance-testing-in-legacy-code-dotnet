using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;
using Commons.Http;
using Commons.Util;
using Commons.WireMock;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public class ClockStubClient : IDisposable
{
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

    public Task<Result<VoidValue, ExtClockErrorResponse>> CheckHealth()
        => _httpClient.Get("/health");

    public Task<Result<ExtGetTimeResponse, ExtClockErrorResponse>> GetTime()
        => _httpClient.Get<ExtGetTimeResponse>("/api/time");

    public async Task<Result<VoidValue, ExtClockErrorResponse>> ConfigureGetTime(ExtGetTimeResponse response)
        => (await _wireMockClient.StubGetAsync("/clock/api/time", HttpStatus.Ok, response))
            .MapError(ExtClockErrorResponse.From);
}
