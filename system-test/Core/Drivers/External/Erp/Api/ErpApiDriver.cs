using System.Net;
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

    public void CreateProduct(string sku, string unitPrice)
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
        
        // ERP API returns 201 Created for successful product creation
        if (response.StatusCode != HttpStatusCode.Created && 
            response.StatusCode != HttpStatusCode.OK)
        {
            var errorContent = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"Failed to create product in ERP. SKU: {sku}, Status: {response.StatusCode}, Error: {errorContent}");
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
