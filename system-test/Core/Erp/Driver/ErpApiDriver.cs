using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public class ErpApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ErpApiClient _erpApiClient;

    public ErpApiDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new HttpGateway(_httpClient, baseUrl);
        _erpApiClient = new ErpApiClient(testHttpClient);
    }

    public Result<VoidResult> GoToErp()
    {
        return _erpApiClient.Health.CheckHealth();
    }

    public Result<VoidResult> CreateProduct(string sku, string price)
    {
        return _erpApiClient.Products.CreateProduct(sku, price);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
