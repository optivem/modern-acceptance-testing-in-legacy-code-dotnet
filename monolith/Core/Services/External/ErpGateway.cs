using System.Text.Json.Serialization;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

public class ErpGateway
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ErpGateway(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ProductDetails?> GetProductDetailsAsync(string sku)
    {
        var erpApiBaseUrl = _configuration["ExternalApis:ErpApi:BaseUrl"];
        var response = await _httpClient.GetAsync($"{erpApiBaseUrl}/products?id={sku}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var products = await response.Content.ReadFromJsonAsync<List<ProductDetails>>();
        return products?.FirstOrDefault();
    }

    public class ProductDetails
    {
        [JsonPropertyName("id")]
        public string Sku { get; set; } = default!;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
