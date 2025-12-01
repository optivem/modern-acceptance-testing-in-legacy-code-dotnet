using System.Net;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;

public class TaxApiDriver : IDisposable
{
    private readonly TaxApiClient _taxApiClient;

    public TaxApiDriver(string baseUrl)
    {
        _taxApiClient = new TaxApiClient(baseUrl);
    }

    public Result<VoidResult> GoToTax()
    {
        return _taxApiClient.Health.CheckHealth();
    }

    public void Dispose()
    {
        _taxApiClient?.Dispose();
    }
}
