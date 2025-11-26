using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.External;

public class ErpApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;

    public ErpApiDriver(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
    }

    public void CreateProduct(string sku, string unitPrice)
    {
        var request = new
        {
            sku = sku,
            unitPrice = unitPrice
        };

        _testHttpClient.Post("/products", request);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
