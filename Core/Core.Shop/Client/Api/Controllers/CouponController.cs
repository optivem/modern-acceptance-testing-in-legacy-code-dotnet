using Commons.Http;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

public class CouponController
{
    private const string Endpoint = "/api/coupons";

    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public CouponController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<VoidValue, ProblemDetailResponse>> PublishCoupon(PublishCouponRequest request)
    {
        return await _httpClient.Post(Endpoint, request);
    }

    public async Task<Result<BrowseCouponsResponse, ProblemDetailResponse>> BrowseCoupons(BrowseCouponsRequest request)
    {
        return await _httpClient.Get<BrowseCouponsResponse>(Endpoint);
    }
}