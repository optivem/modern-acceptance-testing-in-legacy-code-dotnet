using Commons.Util;
using Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Internal;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api;

public class ShopApiDriver : IShopDriver
{
    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;
    private readonly ShopApiClient _apiClient;
    private readonly IOrderDriver _orderDriver;
    private readonly ICouponDriver _couponDriver;

    public ShopApiDriver(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ProblemDetailResponse>(baseUrl);
        _apiClient = new ShopApiClient(_httpClient);
        _orderDriver = new ShopApiOrderDriver(_apiClient);
        _couponDriver = new ShopApiCouponDriver(_apiClient);
    }

    public Result<VoidValue, SystemError> GoToShop()
    {
        var result = _apiClient.Health().CheckHealth().GetAwaiter().GetResult();
        return result.MapError(SystemError.From);
    }

    public IOrderDriver Orders() => _orderDriver;
    
    public ICouponDriver Coupons() => _couponDriver;

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
