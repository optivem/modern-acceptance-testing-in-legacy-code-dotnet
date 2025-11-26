using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Controllers;

public class ProductController
{
    private const string Endpoint = "/products";

    private readonly TestHttpClient _httpClient;

    public ProductController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string CreateProduct(string baseSku, decimal price)
    {
        var uniqueSku = GenerateUniqueSku(baseSku);

        var request = new CreateProductRequest
        {
            Sku = uniqueSku,
            Name = $"Product {uniqueSku}",
            Price = price
        };

        var httpResponse = _httpClient.Post(Endpoint, request);
        var result = TestHttpUtils.GetCreatedResultOrFailure(httpResponse);

        if (!result.Success)
        {
            throw new Exception($"Failed to create product: {string.Join(", ", result.Errors)}");
        }

        return uniqueSku;
    }

    public Result<object?> GetProducts()
    {
        var httpResponse = _httpClient.Get(Endpoint);
        return TestHttpUtils.GetOkResultOrFailure(httpResponse);
    }

    private static string GenerateUniqueSku(string baseSku)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return $"{baseSku}-{timestamp}";
    }
}
