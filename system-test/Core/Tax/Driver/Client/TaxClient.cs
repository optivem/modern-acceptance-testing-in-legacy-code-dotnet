using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Client;

public class TaxClient
{
    private readonly HealthController _healthController;

    public TaxClient(JsonHttpClient httpGateway)
    {
        _healthController = new HealthController(httpGateway);
    }

    public HealthController Health => _healthController;
}
