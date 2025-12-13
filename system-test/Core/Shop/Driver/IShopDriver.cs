using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver;

public interface IShopDriver : IDisposable
{
    Result<VoidValue> GoToShop();
    Result<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request);
    Result<VoidValue> CancelOrder(string orderNumber);
    Result<GetOrderResponse> ViewOrder(string orderNumber);
}
