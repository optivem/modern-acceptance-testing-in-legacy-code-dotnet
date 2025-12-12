using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client;

public class ErpApiClient
{
    private readonly HealthController _healthController;
    private readonly ProductController _productController;

    public ErpApiClient(HttpGateway testHttpClient)
    {
        _healthController = new HealthController(testHttpClient);
        _productController = new ProductController(testHttpClient);
    }

    public ProductController Products => _productController;
    public HealthController Health => _healthController;
}
