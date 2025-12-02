using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;

public class ShopApiDriver : IShopDriver
{
    private readonly HttpClient _httpClient;
    private readonly ShopApiClient _apiClient;

    public ShopApiDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new TestHttpClient(_httpClient, baseUrl);
        _apiClient = new ShopApiClient(testHttpClient);
    }

    public Result<VoidResult> GoToShop()
    {
        return _apiClient.Health().CheckHealth();
    }

    public Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country)
    {
        return _apiClient.Orders().PlaceOrder(sku, quantity, country);
    }

    public Result<VoidResult> CancelOrder(string orderNumber)
    {
        return _apiClient.Orders().CancelOrder(orderNumber);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        return _apiClient.Orders().ViewOrder(orderNumber);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
