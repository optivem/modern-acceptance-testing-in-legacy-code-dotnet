using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api;

public class ShopApiDriver : IShopDriver
{
    private readonly HttpClient _httpClient;
    private readonly ShopApiClient _apiClient;

    public ShopApiDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new HttpGateway(_httpClient, baseUrl);
        _apiClient = new ShopApiClient(testHttpClient);
    }

    public Result<VoidValue> GoToShop()
    {
        return _apiClient.Health().CheckHealth();
    }

    public Result<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request)
    {
        return _apiClient.Orders().PlaceOrder(request);
    }

    public Result<VoidValue> CancelOrder(string orderNumber)
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
