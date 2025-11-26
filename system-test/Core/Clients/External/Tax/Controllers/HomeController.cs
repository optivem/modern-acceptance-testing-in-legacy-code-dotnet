using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Tax.Controllers;

public class HomeController
{
    private const string Endpoint = "/";

    private readonly TestHttpClient _httpClient;

    public HomeController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<object?> Home()
    {
        var httpResponse = _httpClient.Get(Endpoint);
        return TestHttpUtils.GetOkResultOrFailure(httpResponse);
    }
}
