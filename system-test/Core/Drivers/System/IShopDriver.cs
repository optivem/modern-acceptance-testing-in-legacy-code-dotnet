using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

public interface IShopDriver : IDisposable
{
    Result<object?> GoToShop();

    Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country);

    Result<object?> CancelOrder(string orderNumber);

    Result<GetOrderResponse> ViewOrder(string orderNumber);
}
