using System.Text.Json.Serialization;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

public class ProductDetails
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

public class ErpGateway
{
    private readonly HttpClient _httpClient;

    public ErpGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDetails?> GetProductDetailsAsync(string sku)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/{sku}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProductDetails>();
        }
        catch
        {
            return null;
        }
    }
}
