using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Shop.Client.Api;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui;
using Optivem.EShop.SystemTest.Core.Tax.Client;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V2;

public abstract class BaseClientTest : BaseConfigurableTest, IAsyncLifetime
{
    protected readonly SystemConfiguration _configuration;
    
    protected ShopUiClient? _shopUiClient;
    protected ShopApiClient? _shopApiClient;

    protected ErpRealClient? _erpClient;
    protected TaxRealClient? _taxClient;

    protected BaseClientTest() 
    {
        _configuration = LoadConfiguration();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    protected async Task SetUpShopUiClientAsync()
    {
        _shopUiClient = await ShopUiClient.CreateAsync(_configuration.ShopUiBaseUrl);
    }

    protected void SetUpShopApiClient()
    {
        _shopApiClient = new ShopApiClient(_configuration.ShopApiBaseUrl);
    }

    protected void SetUpExternalClients()
    {
        _erpClient = new ErpRealClient(_configuration.ErpBaseUrl);
        _taxClient = new TaxRealClient(_configuration.TaxBaseUrl);
    }

    public virtual async Task DisposeAsync()
    {
        if (_shopUiClient != null)
            await _shopUiClient.DisposeAsync();
        _shopApiClient?.Dispose();    
        _erpClient?.Dispose();
        _taxClient?.Dispose();
    }
}
