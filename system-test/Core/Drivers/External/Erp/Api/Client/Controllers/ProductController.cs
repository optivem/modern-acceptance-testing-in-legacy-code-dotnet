using System.Net;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly TestHttpClient _testHttpClient;

    public ProductController(TestHttpClient testHttpClient)
    {
        _testHttpClient = testHttpClient;
    }

    public Result<VoidResult> CreateProduct(string sku, String price)
    {
        var request = new CreateProductRequest
        {
            Id = sku,
            Title = $"Test product title for {sku}",
            Description = $"Test product description for {sku}",
            Price = price,
            Category = "Test Category",
            Brand = "Test Brand"
        };

        var response = _testHttpClient.Post(Endpoint, request);

        return TestHttpUtils.GetCreatedResultOrFailure(response);
    }
}
