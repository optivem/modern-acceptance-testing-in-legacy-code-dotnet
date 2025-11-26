using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Controllers;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;

public class ErpApiClient : IAsyncDisposable
{
    private readonly HttpClient _client;
    private readonly TestHttpClient _testHttpClient;
    private readonly HomeController _homeController;
    private readonly ProductController _productController;

    public ErpApiClient(string baseUrl)
    {
        _client = new HttpClient();
        _testHttpClient = new TestHttpClient(_client, baseUrl);
        _homeController = new HomeController(_testHttpClient);
        _productController = new ProductController(_testHttpClient);
    }

    public HomeController Home() => _homeController;
    
    public ProductController Products() => _productController;

    public async ValueTask DisposeAsync()
    {
        _client.Dispose();
        await Task.CompletedTask;
    }
}
