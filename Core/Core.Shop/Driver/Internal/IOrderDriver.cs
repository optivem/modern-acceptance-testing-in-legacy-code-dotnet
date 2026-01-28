using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;

public interface IOrderDriver
{
    Result<PlaceOrderResponse, SystemError> PlaceOrder(PlaceOrderRequest request);
    Result<VoidValue, SystemError> CancelOrder(string? orderNumber);
    Result<ViewOrderResponse, SystemError> ViewOrder(string? orderNumber);
}