using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client
{
    public class ErpApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly TestHttpClient _testHttpClient;
        private readonly HealthController _healthController;
        private readonly ProductController _productController;

        public ErpApiClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _testHttpClient = new TestHttpClient(_httpClient, baseUrl);
            _healthController = new HealthController(_testHttpClient);
            _productController = new ProductController(_testHttpClient);
        }

        public ProductController Products => _productController;
        public HealthController Health => _healthController;

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
