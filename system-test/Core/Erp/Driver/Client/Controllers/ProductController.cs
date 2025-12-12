using System.Net;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly HttpGateway _testHttpClient;

    public ProductController(HttpGateway testHttpClient)
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

        return HttpUtils.GetCreatedResultOrFailure(response);
    }
}
