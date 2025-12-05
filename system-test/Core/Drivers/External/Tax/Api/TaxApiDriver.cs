using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;

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
