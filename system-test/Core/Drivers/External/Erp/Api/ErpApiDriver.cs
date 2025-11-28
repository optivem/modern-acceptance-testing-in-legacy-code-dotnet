using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;

public class ErpApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;
    private readonly ProductController _productController;

    public ErpApiDriver(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
        _productController = new ProductController(_testHttpClient);
    }

    public Result<VoidResult> CreateProduct(string id, string price)
    {
        var request = new CreateProductRequest
        {
            Id = id,
            Title = $"Test product title for {id}",
            Description = $"Test product description for {id}",
            Price = price,
            Category = "Test Category",
            Brand = "Test Brand"
        };

        return _productController.CreateProduct(request);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
