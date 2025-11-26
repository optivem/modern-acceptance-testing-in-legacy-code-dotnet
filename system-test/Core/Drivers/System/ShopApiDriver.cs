using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

public class ShopApiDriver : IShopDriver
{
    private readonly ShopApiClient _apiClient;

    public ShopApiDriver(string baseUrl)
    {
        _apiClient = new ShopApiClient(baseUrl);
    }

    public Result<object?> GoToShop()
    {
        return _apiClient.Echo().Echo();
    }

    public Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country)
    {
        return _apiClient.Orders().PlaceOrder(sku, quantity, country);
    }

    public Result<object?> CancelOrder(string orderNumber)
    {
        return _apiClient.Orders().CancelOrder(orderNumber);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        return _apiClient.Orders().ViewOrder(orderNumber);
    }

    public void Dispose()
    {
        _apiClient.Dispose();
    }
}
