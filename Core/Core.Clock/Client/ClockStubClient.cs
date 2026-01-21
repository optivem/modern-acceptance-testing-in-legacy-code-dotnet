using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Commons.Http;
using Optivem.Commons.Util;
using Optivem.Commons.WireMock;

namespace Optivem.EShop.SystemTest.Core.Clock.Client;

public class ClockStubClient : IDisposable
{
    private readonly JsonHttpClient<ClockErrorResponse> _httpClient;
    private readonly JsonWireMockClient _wireMockClient;
    private readonly WireMockManager _wireMockManager;

    public ClockStubClient(string baseUrl)
    {
        var uri = new Uri(baseUrl);
        var httpClient = new HttpClient();
        _httpClient = new JsonHttpClient<ClockErrorResponse>(httpClient, baseUrl);
        _wireMockManager = new WireMockManager(uri.Host, uri.Port);
        _wireMockClient = new JsonWireMockClient(_wireMockManager.Server);
    }

    public Result<GetTimeResponse, ClockErrorResponse> GetTime()
    {
        return _httpClient.Get<GetTimeResponse>("/api/time");
    }

    public void ConfigureGetTime(GetTimeResponse response)
    {
        _wireMockClient.ConfigureGet("/clock/api/time", 200, response);
    }

    public Result<VoidValue, ClockErrorResponse> CheckHealth()
    {
        try
        {
            _httpClient.Get<object>("/health");
            return Result<VoidValue, ClockErrorResponse>.Success(VoidValue.Empty);
        }
        catch (Exception ex)
        {
            var error = new ClockErrorResponse { Message = ex.Message };
            return Result<VoidValue, ClockErrorResponse>.Failure(error);
        }
    }

    public void Dispose()
    {
        _wireMockManager?.Dispose();
    }
}
