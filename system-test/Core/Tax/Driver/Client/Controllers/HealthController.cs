using Optivem.Lang;
using Optivem.Http;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Client.Controllers
{
    public class HealthController
    {
        public static readonly string Endpoint = "/health";

        private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

        public HealthController(JsonHttpClient<ProblemDetailResponse> httpGateway)
        {
            _httpClient = httpGateway;
        }

        public Result<VoidValue, Error> CheckHealth()
        {
            return _httpClient.Get(Endpoint)
                .MapFailure(ProblemDetailConverter.ToError);
        }
    }
}
