using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api.Client
{
    public class TaxApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly TestHttpClient _testHttpClient;
        private readonly HealthController _healthController;

        public TaxApiClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
            _healthController = new HealthController(_testHttpClient);
        }

        public HealthController Health => _healthController;

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
