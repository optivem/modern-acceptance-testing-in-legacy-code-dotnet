using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V3;

/// <summary>
/// V3: Driver-based test infrastructure.
/// Introduces Driver pattern with higher-level abstractions.
/// </summary>
public abstract class BaseDriverTest : BaseConfigurableTest, IAsyncLifetime
{
    protected ErpRealDriver? ErpDriver { get; private set; }
    protected TaxRealDriver? TaxDriver { get; private set; }
    protected IShopDriver? ShopDriver { get; private set; }
    protected SystemConfiguration? Configuration { get; private set; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    protected void SetUpExternalDrivers()
    {
        Configuration = LoadConfiguration();
        ErpDriver = new ErpRealDriver(Configuration.ErpBaseUrl);
        TaxDriver = new TaxRealDriver(Configuration.TaxBaseUrl);
    }

    protected void SetUpShopApiDriver()
    {
        Configuration ??= LoadConfiguration();
        ShopDriver = new ShopApiDriver(Configuration.ShopApiBaseUrl);
    }

    protected async Task SetUpShopUiDriverAsync()
    {
        Configuration ??= LoadConfiguration();
        ShopDriver = await ShopUiDriver.CreateAsync(Configuration.ShopUiBaseUrl);
    }

    public virtual async Task DisposeAsync()
    {
        if (ShopDriver != null)
            await ShopDriver.DisposeAsync();
        
        ErpDriver?.Dispose();
        TaxDriver?.Dispose();
    }
}
