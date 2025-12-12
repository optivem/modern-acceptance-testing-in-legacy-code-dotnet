using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver;

public interface IShopDriver : IDisposable
{
    Result<VoidResult> GoToShop();
    Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country);
    Result<VoidResult> CancelOrder(string orderNumber);
    Result<GetOrderResponse> ViewOrder(string orderNumber);
}
