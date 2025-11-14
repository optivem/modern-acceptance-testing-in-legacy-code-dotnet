using System.Net.Http.Json;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests.Helpers;

public class ErpApiHelper
{
    private readonly TestConfiguration _config;

    public ErpApiHelper(TestConfiguration config)
    {
        _config = config;
    }

    public async Task<string> SetupProductInErp(string baseSku, string title, decimal price)
    {
        // Add UUID suffix to avoid duplicate IDs across test runs
        var uniqueSku = $"{baseSku}-{Guid.NewGuid().ToString().Substring(0, 8)}";

        var product = new ErpProduct(
            Id: uniqueSku,
            Title: title,
            Description: $"Test product for {uniqueSku}",
            Price: price,
            Category: "test-category",
            Brand: "Test Brand"
        );

        var erpApiUrl = _config.BaseUrl.Replace(":8081", ":3100");
        using var erpClient = new HttpClient { BaseAddress = new Uri(erpApiUrl) };

        var response = await erpClient.PostAsJsonAsync("/products", product);
        
        // JSON Server returns 201 Created for successful resource creation
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created,
            $"ERP product setup should succeed. Status: {response.StatusCode}, Body: {await response.Content.ReadAsStringAsync()}");

        return uniqueSku;
    }

    protected record ErpProduct(
        string Id,
        string Title,
        string Description,
        decimal Price,
        string Category,
        string Brand
    );
}
