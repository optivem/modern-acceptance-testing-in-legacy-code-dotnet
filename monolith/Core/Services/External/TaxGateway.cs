using System.Text.Json.Serialization;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

public class TaxGateway
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TaxGateway(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<CountryDetails?> GetTaxDetailsAsync(string country)
    {
        var taxApiBaseUrl = _configuration["ExternalApis:TaxApi:BaseUrl"];
        var response = await _httpClient.GetAsync($"{taxApiBaseUrl}/countries?code={country}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var countries = await response.Content.ReadFromJsonAsync<List<CountryDetails>>();
        return countries?.FirstOrDefault();
    }

    public class CountryDetails
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = default!;

        [JsonPropertyName("taxRate")]
        public decimal TaxRate { get; set; }
    }
}
