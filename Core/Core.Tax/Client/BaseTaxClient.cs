using Commons.Http;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Client;

public abstract class BaseTaxClient : IDisposable
{
    private const string HealthEndpoint = "/health";
    private const string CountriesEndpoint = "/api/countries";

    protected readonly JsonHttpClient<ExtTaxErrorResponse> _httpClient;

    protected BaseTaxClient(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ExtTaxErrorResponse>(baseUrl);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public Task<Result<VoidValue, ExtTaxErrorResponse>> CheckHealth()
        => _httpClient.Get(HealthEndpoint);

    public Task<Result<ExtCountryDetailsResponse, ExtTaxErrorResponse>> GetCountry(string? country)
        => _httpClient.Get<ExtCountryDetailsResponse>($"{CountriesEndpoint}/{country}");
}