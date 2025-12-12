using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Api.Client;

public class ShopApiClient
{
    private readonly HealthController _healthController;
    private readonly OrderController _orderController;

    public ShopApiClient(HttpGateway testHttpClient)
    {
        _healthController = new HealthController(testHttpClient);
        _orderController = new OrderController(testHttpClient);
    }

    public HealthController Health() => _healthController;

    public OrderController Orders() => _orderController;
}
