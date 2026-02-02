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
    protected SystemConfiguration? Configuration;

    protected ShopUiClient? ShopUiClient;
    protected ShopApiClient? ShopApiClient;
    protected ErpRealClient? ErpClient;
    protected TaxRealClient? TaxClient;

    protected void SetUpConfiguration()
    {
        Configuration = LoadConfiguration();
    }

    protected void SetUpShopUiClient()
    {
        ShopUiClient = ShopUiClient.CreateAsync(Configuration!.ShopUiBaseUrl).Result;
    }

    protected void SetUpShopApiClient()
    {
        var httpClient = new Commons.Http.JsonHttpClient<Optivem.EShop.SystemTest.Core.Shop.Client.Api.Dtos.Errors.ProblemDetailResponse>(
            Configuration!.ShopApiBaseUrl);
        ShopApiClient = new ShopApiClient(httpClient);
    }

    protected void SetUpExternalClients()
    {
        ErpClient = new ErpRealClient(Configuration!.ErpBaseUrl);
        TaxClient = new TaxRealClient(Configuration!.TaxBaseUrl);
    }

    protected virtual void TearDown()
    {
        ShopUiClient?.DisposeAsync().AsTask().Wait();
        ErpClient?.Dispose();
        TaxClient?.Dispose();
    }
}
