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
    protected IShopDriver? ShopDriver { get; private set; }
    protected ErpRealDriver? ErpDriver { get; private set; }
    protected TaxRealDriver? TaxDriver { get; private set; }

    public virtual async Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        ShopDriver = await CreateShopDriverAsync(configuration);
        ErpDriver = new ErpRealDriver(configuration.ErpBaseUrl);
        TaxDriver = new TaxRealDriver(configuration.TaxBaseUrl);
    }

    public virtual async Task DisposeAsync()
    {
        if (ShopDriver != null)
            await ShopDriver.DisposeAsync();
        
        ErpDriver?.Dispose();
        TaxDriver?.Dispose();
    }

    private async Task<IShopDriver?> CreateShopDriverAsync(SystemConfiguration configuration)
    {
        var channelType = "API";
        
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
