using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Controllers;

public class HomeController
{
    private const string Endpoint = "/";

    private readonly TestHttpClient _httpClient;

    public HomeController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> HomeAsync()
    {
        return await _httpClient.GetAsync(Endpoint);
    }

    public void AssertHomeSuccessful(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertOk(httpResponse);
    }
}
