using Optivem.Lang;
using Optivem.Http;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public class ErpDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ErpClient _erpClient;

    public ErpDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var httpGateway = new HttpGateway(_httpClient, baseUrl);
        _erpClient = new ErpClient(httpGateway);
    }

    public Result<VoidValue, Error> GoToErp()
    {
        return _erpClient.Health.CheckHealth();
    }

    public Result<VoidValue, Error> CreateProduct(CreateProductRequest request)
    {
        return _erpClient.Products.CreateProduct(request);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
