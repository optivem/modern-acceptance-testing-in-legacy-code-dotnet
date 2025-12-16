using Optivem.Lang;
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
        var testHttpClient = new JsonHttpClient(_httpClient, baseUrl);
        _apiClient = new ShopApiClient(testHttpClient);
    }

    public Result<VoidValue, Error> GoToShop()
    {
        return _apiClient.Health().CheckHealth();
    }

    public Result<PlaceOrderResponse, Error> PlaceOrder(PlaceOrderRequest request)
    {
        return _apiClient.Orders().PlaceOrder(request);
    }

    public Result<VoidValue, Error> CancelOrder(string orderNumber)
    {
        return _apiClient.Orders().CancelOrder(orderNumber);
    }

    public Result<GetOrderResponse, Error> ViewOrder(string orderNumber)
    {
        return _apiClient.Orders().ViewOrder(orderNumber);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
