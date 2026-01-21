using Optivem.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Client;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public class TaxDriver : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TaxClient _taxApiClient;

    public TaxDriver(string baseUrl)
    {
        _httpClient = HttpClientFactory.Create(baseUrl);
        var testHttpClient = new JsonHttpClient<ProblemDetailResponse>(_httpClient, baseUrl);
        _taxApiClient = new TaxClient(testHttpClient);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public Result<VoidValue, Error> GoToTax()
    {
        return _taxApiClient.Health.CheckHealth();
    }

}
