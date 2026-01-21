using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api.Controllers;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Api;

public class ShopApiClient
{
    private readonly HealthController _healthController;
    private readonly OrderController _orderController;

    public ShopApiClient(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _healthController = new HealthController(httpClient);
        _orderController = new OrderController(httpClient);
    }

    public HealthController Health() => _healthController;

    public OrderController Orders() => _orderController;
}
