using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.System;

public interface IShopDriver : IDisposable
{
    Result<VoidResult> GoToShop();
    Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country);
    Result<VoidResult> CancelOrder(string orderNumber);
    Result<GetOrderResponse> ViewOrder(string orderNumber);
}
