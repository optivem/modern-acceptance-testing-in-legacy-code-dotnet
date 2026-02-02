using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V4;


public abstract class BaseChannelDriverTest : BaseConfigurableTest, IAsyncLifetime
{
    protected IShopDriver? _shopDriver;
    protected ErpRealDriver? _erpDriver;
    protected TaxRealDriver? _taxDriver;

    public virtual async Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        
        _shopDriver = await CreateShopDriverAsync(configuration);   
        _erpDriver = new ErpRealDriver(configuration.ErpBaseUrl);
        _taxDriver = new TaxRealDriver(configuration.TaxBaseUrl);
    }

    public virtual async Task DisposeAsync()
    {
        if (_shopDriver != null)
            await _shopDriver.DisposeAsync();
        
        _erpDriver?.Dispose();
        _taxDriver?.Dispose();
    }

    private async Task<IShopDriver?> CreateShopDriverAsync(SystemConfiguration configuration)
    {
        // TODO: VJ: Should be dynamic
        var channelType = ChannelContext.Get();
        
        if (channelType == ChannelType.UI)
        {
            return await ShopUiDriver.CreateAsync(configuration.ShopUiBaseUrl);
        }
        else if (channelType == ChannelType.API)
        {
            return new ShopApiDriver(configuration.ShopApiBaseUrl);
        }
        else
        {
            throw new InvalidOperationException($"Unknown channel: {channelType}");
        }
    }
}
