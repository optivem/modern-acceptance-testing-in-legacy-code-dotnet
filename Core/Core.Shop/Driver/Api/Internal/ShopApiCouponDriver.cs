using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Commons;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Internal;

public class ShopApiCouponDriver : ICouponDriver
{
    private readonly ShopApiClient _apiClient;

    public ShopApiCouponDriver(ShopApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Result<VoidValue, SystemError> PublishCoupon(PublishCouponRequest request)
    {
        return _apiClient.Coupons().PublishCoupon(request).MapError(SystemError.From);
    }

    public Result<BrowseCouponsResponse, SystemError> BrowseCoupons(BrowseCouponsRequest request)
    {
        return _apiClient.Coupons().BrowseCoupons(request).MapError(SystemError.From);
    }
}