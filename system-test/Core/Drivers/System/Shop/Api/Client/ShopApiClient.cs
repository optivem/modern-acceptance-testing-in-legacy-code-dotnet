using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api.Client;

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
