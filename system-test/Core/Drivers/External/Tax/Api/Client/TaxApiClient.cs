using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client.Controllers;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client;

public class TaxApiClient
{
    private readonly HealthController _healthController;

    public TaxApiClient(HttpGateway testHttpClient)
    {
        _healthController = new HealthController(testHttpClient);
    }

    public HealthController Health => _healthController;
}
