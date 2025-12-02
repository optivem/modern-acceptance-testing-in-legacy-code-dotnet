using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client;

public class ErpApiClient
{
    private readonly HealthController _healthController;
    private readonly ProductController _productController;

    public ErpApiClient(TestHttpClient testHttpClient)
    {
        _healthController = new HealthController(testHttpClient);
        _productController = new ProductController(testHttpClient);
    }

    public ProductController Products => _productController;
    public HealthController Health => _healthController;
}
