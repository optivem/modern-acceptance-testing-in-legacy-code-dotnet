using System.Net;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;

public class TaxApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;

    public TaxApiDriver(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl + "/api");
    }

    public void CreateTaxRate(string country, string taxRate)
    {
        var request = new CreateTaxRateRequest
        {
            Country = country,
            TaxRate = taxRate
        };

        var response = _testHttpClient.Post("/tax-rates", request);
        
        // Tax API returns 201 Created for successful tax rate creation
        if (response.StatusCode != HttpStatusCode.Created && 
            response.StatusCode != HttpStatusCode.OK)
        {
            var errorContent = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"Failed to create tax rate. Country: {country}, Status: {response.StatusCode}, Error: {errorContent}");
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
