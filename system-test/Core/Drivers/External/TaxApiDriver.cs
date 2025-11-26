using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.External;

public class TaxApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestHttpClient _testHttpClient;

    public TaxApiDriver(string baseUrl)
    {
        _httpClient = new HttpClient();
        _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
    }

    public void SetTaxRate(string country, string taxRate)
    {
        var request = new
        {
            country = country,
            taxRate = taxRate
        };

        _testHttpClient.Post("/tax-rates", request);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
