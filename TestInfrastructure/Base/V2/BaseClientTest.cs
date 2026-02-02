using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui;
using Optivem.EShop.SystemTest.Core.Tax.Client;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V2;

public abstract class BaseClientTest : BaseConfigurableTest
{
    protected ErpRealClient? ErpClient;
    protected TaxRealClient? TaxClient;
    protected ShopApiClient? ShopApiClient;
    protected ShopUiClient? ShopUiClient;
    protected Commons.Http.JsonHttpClient<Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors.ProblemDetailResponse>? ShopHttpClient;
    protected SystemConfiguration? Configuration;

    protected void SetUpExternalClients()
    {
        Configuration = LoadConfiguration();
        ErpClient = new ErpRealClient(Configuration.ErpBaseUrl);
        TaxClient = new TaxRealClient(Configuration.TaxBaseUrl);
    }

    protected void SetUpShopApiClient()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }
        ShopHttpClient = new Commons.Http.JsonHttpClient<Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors.ProblemDetailResponse>(
            Configuration.ShopApiBaseUrl);
        ShopApiClient = new ShopApiClient(ShopHttpClient);
    }

    protected void SetUpShopUiClient()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }
        ShopUiClient = ShopUiClient.CreateAsync(Configuration.ShopUiBaseUrl).Result;
    }

    protected virtual void TearDown()
    {
        ShopUiClient?.DisposeAsync().AsTask().Wait();
        ErpClient?.Dispose();
        TaxClient?.Dispose();
        ShopHttpClient?.Dispose();
    }
}
