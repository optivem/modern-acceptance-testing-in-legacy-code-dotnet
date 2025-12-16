using Optivem.Lang;
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

        private readonly JsonHttpClient _testHttpClient;

        public HealthController(JsonHttpClient testHttpClient)
        {
            _testHttpClient = testHttpClient;
        }

        public Result<VoidValue, Error> CheckHealth()
        {
            var response = _testHttpClient.Get(Endpoint);
            return HttpUtils.GetOkResultOrFailure(response);
        }
    }
}
