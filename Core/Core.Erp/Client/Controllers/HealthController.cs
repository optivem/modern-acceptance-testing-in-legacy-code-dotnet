using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client.Controllers
{
    public class HealthController
    {
        private static readonly string Endpoint = "/health";

        private readonly JsonHttpClient<ExtErpErrorResponse> _httpClient;

        public HealthController(JsonHttpClient<ExtErpErrorResponse> httpClient)
        {
            _httpClient = httpClient;
        }

        public Result<VoidValue, ExtErpErrorResponse> CheckHealth()
        {
            return _httpClient.Get(Endpoint);
        }
    }
}
