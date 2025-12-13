using Optivem.Lang;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver;

public interface IShopDriver : IDisposable
{
    Result<VoidValue, Error> GoToShop();
    Result<PlaceOrderResponse, Error> PlaceOrder(PlaceOrderRequest request);
    Result<VoidValue, Error> CancelOrder(string orderNumber);
    Result<GetOrderResponse, Error> ViewOrder(string orderNumber);
}
