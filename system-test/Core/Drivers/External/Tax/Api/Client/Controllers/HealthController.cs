using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client.Controllers
{
    public class HealthController
    {
        public static readonly string Endpoint = "/health";

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
