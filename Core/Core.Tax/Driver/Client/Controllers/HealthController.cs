using Optivem.Lang;
using Optivem.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Client.Controllers
{
    public class HealthController
    {
        public static readonly string Endpoint = "/health";

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
