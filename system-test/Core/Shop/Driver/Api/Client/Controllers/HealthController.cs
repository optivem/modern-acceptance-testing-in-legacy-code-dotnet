using Optivem.Lang;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client.Controllers;

public class HealthController
{
    private const string Endpoint = "/health";

    private readonly HttpGateway _httpClient;

    public HealthController(HttpGateway httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidValue, Error> CheckHealth()
    {
        var httpResponse = _httpClient.Get(Endpoint);
        return HttpUtils.GetOkResultOrFailure(httpResponse);
    }
}
