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

    public async Task<Result<VoidValue, ExtClockErrorResponse>> CheckHealth()
    {
        return await _httpClient.Get<VoidValue>("/health");
    }

    public async Task<Result<ExtGetTimeResponse, ExtClockErrorResponse>> GetTime()
    {
        return await _httpClient.Get<ExtGetTimeResponse>("/api/time");
    }

    public async Task<Result<VoidValue, ExtClockErrorResponse>> ConfigureGetTime(ExtGetTimeResponse response)
    {
        var result = await _wireMockClient.StubGetAsync("/clock/api/time", 200, response);
        return result.MapError(ExtClockErrorResponse.From);
    }
}
