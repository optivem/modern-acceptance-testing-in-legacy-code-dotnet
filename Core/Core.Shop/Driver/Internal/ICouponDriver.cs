using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;

public interface ICouponDriver
{
    Result<VoidValue, SystemError> PublishCoupon(PublishCouponRequest request);
    Result<BrowseCouponsResponse, SystemError> BrowseCoupons(BrowseCouponsRequest request);
}