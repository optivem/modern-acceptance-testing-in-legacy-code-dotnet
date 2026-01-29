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

    public Result<PlaceOrderResponse, SystemError> PlaceOrder(PlaceOrderRequest request)
    {
        var result = _apiClient.Orders().PlaceOrder(request).GetAwaiter().GetResult();
        return result.MapError(SystemError.From);
    }

    public Result<VoidValue, SystemError> CancelOrder(string? orderNumber)
    {
        var result = _apiClient.Orders().CancelOrder(orderNumber).GetAwaiter().GetResult();
        return result.MapError(SystemError.From);
    }

    public Result<ViewOrderResponse, SystemError> ViewOrder(string? orderNumber)
    {
        var result = _apiClient.Orders().ViewOrder(orderNumber).GetAwaiter().GetResult();
        return result.MapError(SystemError.From);
    }
}