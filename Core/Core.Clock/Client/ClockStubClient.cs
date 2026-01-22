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
    private readonly WireMockManager _wireMockManager;

    public ClockStubClient(string baseUrl)
    {
        var uri = new Uri(baseUrl);
        var httpClient = new HttpClient();
        _httpClient = new JsonHttpClient<ExtClockErrorResponse>(httpClient, baseUrl);
        _wireMockManager = new WireMockManager(uri.Host, uri.Port);
        _wireMockClient = new JsonWireMockClient(_wireMockManager.Server);
    }

    public void Dispose()
    {
        _wireMockManager?.Dispose();
    }

    public Result<VoidValue, ExtClockErrorResponse> CheckHealth()
    {
        return _httpClient.Get<VoidValue>("/health");
    }

    public Result<ExtGetTimeResponse, ExtClockErrorResponse> GetTime()
    {
        return _httpClient.Get<ExtGetTimeResponse>("/api/time");
    }

    public void ConfigureGetTime(ExtGetTimeResponse response)
    {
        _wireMockClient.ConfigureGet("/clock/api/time", 200, response);
    }
}
