using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers
{
    public class HealthController
    {
        private static readonly string Endpoint = "/health";

        private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

        public HealthController(JsonHttpClient<ProblemDetailResponse> httpClient)
        {
            _httpClient = httpClient;
        }

        public Result<VoidValue, Error> CheckHealth()
        {
            return _httpClient.Get(Endpoint)
                .MapFailure(ProblemDetailConverter.ToError);
        }
    }
}
