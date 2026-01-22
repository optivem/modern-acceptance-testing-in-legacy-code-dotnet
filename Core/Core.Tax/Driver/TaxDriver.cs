using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Tax.Client;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public class TaxDriver : IDisposable
{
    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;
    private readonly TaxClient _taxApiClient;

    public TaxDriver(string baseUrl)
    {
        _httpClient = new JsonHttpClient<ProblemDetailResponse>(baseUrl);
        _taxApiClient = new TaxClient(_httpClient);
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
