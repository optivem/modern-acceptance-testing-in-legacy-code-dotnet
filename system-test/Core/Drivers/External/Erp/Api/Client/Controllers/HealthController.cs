using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers
{
    public class HealthController
    {
        private static readonly string Endpoint = "/health";

        private readonly HttpGateway _testHttpClient;

        public HealthController(HttpGateway testHttpClient)
        {
            _testHttpClient = testHttpClient;
        }

        public Result<VoidResult> CheckHealth()
        {
            var response = _testHttpClient.Get(Endpoint);
            return HttpUtils.GetOkResultOrFailure(response);
        }
    }
}
