using Commons.Util;
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

    public Task<Result<VoidValue, SystemError>> PublishCoupon(PublishCouponRequest request)
        => _apiClient.Coupons().PublishCoupon(request)
            .MapErrorAsync(SystemError.From);

    public Task<Result<BrowseCouponsResponse, SystemError>> BrowseCoupons(BrowseCouponsRequest request)
        => _apiClient.Coupons().BrowseCoupons(request)
            .MapErrorAsync(SystemError.From);
}