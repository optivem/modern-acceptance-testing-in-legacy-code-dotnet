using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Controllers;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;

public class ShopApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;
    private readonly EchoController _echoController;
    private readonly OrderController _orderController;

    public ShopApiClient(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
        _echoController = new EchoController(_testHttpClient);
        _orderController = new OrderController(_testHttpClient);
    }

    public EchoController Echo() => _echoController;

    public OrderController Orders() => _orderController;

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
