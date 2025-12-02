using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;

public class ErpApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ErpApiClient _erpApiClient;

    public ErpApiDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new TestHttpClient(_httpClient, baseUrl);
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
