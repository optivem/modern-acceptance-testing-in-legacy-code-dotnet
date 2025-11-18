using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Dtos;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Controllers;

public class ProductController
{
    private const string Endpoint = "products";

    private readonly TestHttpClient _httpClient;

    public ProductController(TestHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CreateProductAsync(string baseSku, decimal price)
    {
        // Add UUID suffix to avoid duplicate IDs across test runs
        var uniqueSku = $"{baseSku}-{Guid.NewGuid().ToString().Substring(0, 8)}";

        var product = new CreateProductRequest
        {
            Id = uniqueSku,
            Title = $"Test product title for {uniqueSku}",
            Description = $"Test product description for {uniqueSku}",
            Price = price,
            Category = "Test Category",
            Brand = "Test Brand"
        };

        var httpResponse = await _httpClient.PostAsync(Endpoint, product);
        _httpClient.AssertCreated(httpResponse);

        return uniqueSku;
    }
}
