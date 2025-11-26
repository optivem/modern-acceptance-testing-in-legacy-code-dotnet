using System.Text.Json.Serialization;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

public class CountryDetails
{
    [JsonPropertyName("taxRate")]
    public decimal TaxRate { get; set; }
}

public class TaxGateway
{
    private readonly HttpClient _httpClient;

    public TaxGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CountryDetails?> GetTaxDetailsAsync(string country)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/countries/{country}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CountryDetails>();
        }
        catch
        {
            return null;
        }
    }
}
