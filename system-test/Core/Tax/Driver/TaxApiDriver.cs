using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Client;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public class TaxApiDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TaxApiClient _taxApiClient;

    public TaxApiDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new HttpGateway(_httpClient, baseUrl);
        _taxApiClient = new TaxApiClient(testHttpClient);
    }

    public Result<VoidResult> GoToTax()
    {
        return _taxApiClient.Health.CheckHealth();
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
