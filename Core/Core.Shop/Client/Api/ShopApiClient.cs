using Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api;

public class ShopApiClient : IDisposable
{
    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;
    private readonly HealthController _healthController;
    private readonly OrderController _orderController;
    private readonly CouponController _couponController;

    public ShopApiClient(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ProblemDetailResponse>(baseUrl);
        _healthController = new HealthController(_httpClient);
        _orderController = new OrderController(_httpClient);
        _couponController = new CouponController(_httpClient);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public HealthController Health() => _healthController;

    public OrderController Orders() => _orderController;

    public CouponController Coupons() => _couponController;


}
