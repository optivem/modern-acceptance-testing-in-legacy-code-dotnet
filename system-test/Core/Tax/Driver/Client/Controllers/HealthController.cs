using Optivem.Lang;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Client.Controllers
{
    public class HealthController
    {
        public static readonly string Endpoint = "/health";

        private readonly HttpGateway _httpClient;

        public HealthController(HttpGateway httpGateway)
        {
            _httpClient = httpGateway;
        }

        public Result<VoidValue, Error> CheckHealth()
        {
            var response = _httpClient.Get(Endpoint);
            return HttpUtils.GetOkResultOrFailure(response);
        }
    }
}
