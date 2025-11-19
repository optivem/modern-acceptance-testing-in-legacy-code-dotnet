using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Tax.Controllers;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Tax;

public class TaxApiClient : IAsyncDisposable
{
    private readonly HttpClient _client;
    private readonly TestHttpClient _testHttpClient;
    private readonly HomeController _homeController;

    public TaxApiClient(string baseUrl)
    {
        _client = new HttpClient();
        _testHttpClient = new TestHttpClient(_client, baseUrl);
        _homeController = new HomeController(_testHttpClient);
    }

    public HomeController Home() => _homeController;

    public async ValueTask DisposeAsync()
    {
        _client.Dispose();
        await Task.CompletedTask;
    }
}
