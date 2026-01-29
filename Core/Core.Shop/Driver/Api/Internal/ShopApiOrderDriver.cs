using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Internal;

public class ShopApiOrderDriver : IOrderDriver
{
    private readonly ShopApiClient _apiClient;

    public ShopApiOrderDriver(ShopApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Result<PlaceOrderResponse, SystemError>> PlaceOrder(PlaceOrderRequest request)
    {
        var result = await _apiClient.Orders().PlaceOrder(request);
        return result.MapError(SystemError.From);
    }

    public async Task<Result<VoidValue, SystemError>> CancelOrder(string? orderNumber)
    {
        var result = await _apiClient.Orders().CancelOrder(orderNumber);
        return result.MapError(SystemError.From);
    }

    public async Task<Result<ViewOrderResponse, SystemError>> ViewOrder(string? orderNumber)
    {
        var result = await _apiClient.Orders().ViewOrder(orderNumber);
        return result.MapError(SystemError.From);
    }
}