using Optivem.Util;
using Optivem.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client.Controllers;

public class HealthController
{
    private const string Endpoint = "/health";

    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public HealthController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidValue, Error> CheckHealth()
    {
        return _httpClient.Get(Endpoint)
            .MapFailure(ProblemDetailConverter.ToError);
    }
}
