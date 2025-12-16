using Optivem.Lang;
using Optivem.Http;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers
{
    public class HealthController
    {
        private static readonly string Endpoint = "/health";

        private readonly JsonHttpClient<ProblemDetailResponse> _testHttpClient;

        public HealthController(JsonHttpClient<ProblemDetailResponse> testHttpClient)
        {
            _testHttpClient = testHttpClient;
        }

        public Result<VoidValue, Error> CheckHealth()
        {
            return _testHttpClient.Get(Endpoint)
                .MapFailure(ProblemDetailConverter.ToError);
        }
    }
}
