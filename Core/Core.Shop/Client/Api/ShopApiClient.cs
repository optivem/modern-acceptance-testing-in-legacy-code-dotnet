using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api;

public class ShopApiClient
{
    private readonly HealthController _healthController;
    private readonly OrderController _orderController;
    private readonly CouponController _couponController;

    public ShopApiClient(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _healthController = new HealthController(httpClient);
        _orderController = new OrderController(httpClient);
        _couponController = new CouponController(httpClient);
    }

    public HealthController Health() => _healthController;

    public OrderController Orders() => _orderController;

    public CouponController Coupons() => _couponController;
}
