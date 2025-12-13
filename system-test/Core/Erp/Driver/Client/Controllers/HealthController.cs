using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers
{
    public class HealthController
    {
        private static readonly string Endpoint = "/health";

        private readonly HttpGateway _testHttpClient;

        public HealthController(HttpGateway testHttpClient)
        {
            _testHttpClient = testHttpClient;
        }

        public Result<VoidValue> CheckHealth()
        {
            var response = _testHttpClient.Get(Endpoint);
            return HttpUtils.GetOkResultOrFailure(response);
        }
    }
}
