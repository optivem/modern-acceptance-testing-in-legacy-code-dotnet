using Commons.Http;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Client;

public abstract class BaseTaxClient : IDisposable
{
    protected readonly JsonHttpClient<ExtTaxErrorResponse> _httpClient;

    protected BaseTaxClient(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ExtTaxErrorResponse>(baseUrl);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public async Task<Result<VoidValue, ExtTaxErrorResponse>> CheckHealth()
    {
        return await _httpClient.Get("/health");
    }

    public async Task<Result<ExtCountryDetailsResponse, ExtTaxErrorResponse>> GetCountry(string country)
    {
        return await _httpClient.Get<ExtCountryDetailsResponse>($"/api/countries/{country}");
    }
}