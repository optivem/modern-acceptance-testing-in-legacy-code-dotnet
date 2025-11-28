using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client.Controllers;

public class HealthController
{
    private const string Endpoint = "/health";

    private readonly TestHttpClient _httpClient;

    public HealthController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidResult> CheckHealth()
    {
        var httpResponse = _httpClient.Get(Endpoint);
        return TestHttpUtils.GetOkResultOrFailure(httpResponse);
    }
}
