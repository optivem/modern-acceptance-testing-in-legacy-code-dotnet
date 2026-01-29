using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;

public interface IOrderDriver
{
    Task<Result<PlaceOrderResponse, SystemError>> PlaceOrder(PlaceOrderRequest request);
    Task<Result<VoidValue, SystemError>> CancelOrder(string? orderNumber);
    Task<Result<ViewOrderResponse, SystemError>> ViewOrder(string? orderNumber);
}