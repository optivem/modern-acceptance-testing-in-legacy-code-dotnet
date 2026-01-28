using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver;

public interface IShopDriver : IDisposable
{
    Result<VoidValue, SystemError> GoToShop();
    IOrderDriver Orders();
    ICouponDriver Coupons();
}
