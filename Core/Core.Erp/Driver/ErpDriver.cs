using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Requests;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public class ErpDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ErpClient _erpClient;

    public ErpDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var httpGateway = new JsonHttpClient<ExtErpErrorResponse>(_httpClient, baseUrl);
        _erpClient = new ErpClient(httpGateway);
    }

    public Result<VoidValue, ErpErrorResponse> GoToErp()
    {
        return _erpClient.Health.CheckHealth()
            .MapFailure(ErpErrorResponse.From);
    }

    public Result<VoidValue, ErpErrorResponse> CreateProduct(CreateProductRequest request)
    {
        return _erpClient.Products.CreateProduct(request)
            .MapFailure(ErpErrorResponse.From);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
