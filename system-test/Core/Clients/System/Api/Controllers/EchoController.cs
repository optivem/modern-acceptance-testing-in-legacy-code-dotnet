using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Controllers;

public class EchoController
{
    private const string Endpoint = "/api/echo";

    private readonly TestHttpClient _httpClient;

    public EchoController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<object?> Echo()
    {
        var httpResponse = _httpClient.Get(Endpoint);
        return TestHttpUtils.GetOkResultOrFailure(httpResponse);
    }
}
