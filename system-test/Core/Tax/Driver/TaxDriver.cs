using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Client;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public class TaxDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TaxClient _taxApiClient;

    public TaxDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new HttpGateway(_httpClient, baseUrl);
        _taxApiClient = new TaxClient(testHttpClient);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public Result<VoidValue> GoToTax()
    {
        return _taxApiClient.Health.CheckHealth();
    }

}
