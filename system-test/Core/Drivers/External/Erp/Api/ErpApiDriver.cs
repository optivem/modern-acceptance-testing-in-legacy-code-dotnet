using System.Net;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;

public class ErpApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;

    public ErpApiDriver(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl + "/api");
    }

    public Result<VoidResult> CreateProduct(string sku, string unitPrice)
    {
        var request = new CreateProductRequest
        {
            Id = sku,
            Title = $"Test product title for {sku}",
            Description = $"Test product description for {sku}",
            Price = unitPrice,
            Category = "Test Category",
            Brand = "Test Brand"
        };

        var response = _testHttpClient.Post("/products", request);
        
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var errorContent = response.Content.ReadAsStringAsync().Result;
            return Result<VoidResult>.FailureResult(new List<string> { $"Failed to create product in ERP. SKU: {sku}, Status: {response.StatusCode}, Error: {errorContent}" });
        }

        return Result<VoidResult>.SuccessResult(new VoidResult());
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
