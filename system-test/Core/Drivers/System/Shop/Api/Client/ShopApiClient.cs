using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client;

public class ShopApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _apiHttpClient;
    private readonly TestHttpClient _healthHttpClient;
    private readonly HealthController _healthController;
    private readonly OrderController _orderController;

    public ShopApiClient(string baseUrl)
    {
        _httpClient = new HttpClient();
        
        // Health endpoint is at root (e.g., http://localhost:8081/health)
        var healthBaseUrl = baseUrl.Replace("/api", "");
        _healthHttpClient = new TestHttpClient(_httpClient, healthBaseUrl);
        _healthController = new HealthController(_healthHttpClient);
        
        // API endpoints are under /api (e.g., http://localhost:8081/api/orders)
        _apiHttpClient = new TestHttpClient(_httpClient, baseUrl);
        _orderController = new OrderController(_apiHttpClient);
    }

    public HealthController Health() => _healthController;

    public OrderController Orders() => _orderController;

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
