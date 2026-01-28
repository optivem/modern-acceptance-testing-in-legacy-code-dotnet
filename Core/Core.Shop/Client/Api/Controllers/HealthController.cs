using Commons.Util;
using Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

public class HealthController
{
    private const string Endpoint = "/health";

    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public HealthController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidValue, ProblemDetailResponse> CheckHealth()
    {
        return _httpClient.Get(Endpoint);
    }
}
