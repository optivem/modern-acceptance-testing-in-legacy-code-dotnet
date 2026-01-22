using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;
using Optivem.Commons.Http;
using Optivem.Commons.Util;
using Optivem.Commons.WireMock;

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
        // No need to dispose WireMock client - it just connects to running server
    }

    public Result<VoidValue, ExtClockErrorResponse> CheckHealth()
    {
        return _httpClient.Get<VoidValue>("/health");
    }

    public Result<ExtGetTimeResponse, ExtClockErrorResponse> GetTime()
    {
        return _httpClient.Get<ExtGetTimeResponse>("/api/time");
    }

    public Result<VoidValue, ExtClockErrorResponse> ConfigureGetTime(ExtGetTimeResponse response)
    {
        return _wireMockClient.StubGet("/clock/api/time", 200, response)
            .MapFailure(ExtClockErrorResponse.From);
    }
}
