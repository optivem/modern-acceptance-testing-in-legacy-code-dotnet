using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Client;

public class TaxApiClient
{
    private readonly HealthController _healthController;

    public TaxApiClient(HttpGateway testHttpClient)
    {
        _healthController = new HealthController(testHttpClient);
    }

    public HealthController Health => _healthController;
}
