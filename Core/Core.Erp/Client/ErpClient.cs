using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Erp.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpClient
{
    private readonly HealthController _healthController;
    private readonly ProductController _productController;

    public ErpClient(JsonHttpClient<ProblemDetailResponse> httpGateway)
    {
        _healthController = new HealthController(httpGateway);
        _productController = new ProductController(httpGateway);
    }

    public ProductController Products => _productController;
    public HealthController Health => _healthController;
}
