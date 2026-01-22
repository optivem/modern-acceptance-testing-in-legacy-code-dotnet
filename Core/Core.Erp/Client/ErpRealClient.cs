using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Erp.Client.Controllers;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpRealClient
{
    private readonly HealthController _healthController;
    private readonly ProductController _productController;

    public ErpRealClient(JsonHttpClient<ExtErpErrorResponse> httpGateway)
    {
        _healthController = new HealthController(httpGateway);
        _productController = new ProductController(httpGateway);
    }

    public ProductController Products => _productController;
    public HealthController Health => _healthController;
}