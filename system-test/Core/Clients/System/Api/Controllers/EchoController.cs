using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Controllers;

public class EchoController
{
    private const string Endpoint = "/echo";

    private readonly TestHttpClient _httpClient;

    public EchoController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> EchoAsync()
    {
        return await _httpClient.GetAsync(Endpoint);
    }

    public void AssertEchoSuccessful(HttpResponseMessage httpResponse)
    {
        _httpClient.AssertOk(httpResponse);
    }
}
