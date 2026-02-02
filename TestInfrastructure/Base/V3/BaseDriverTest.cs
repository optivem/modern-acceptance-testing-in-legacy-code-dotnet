using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V3;

public abstract class BaseDriverTest : BaseConfigurableTest
{
    protected ErpRealDriver? ErpDriver;
    protected TaxRealDriver? TaxDriver;
    protected IShopDriver? ShopDriver;
    protected SystemConfiguration? Configuration;

    protected void SetUpExternalDrivers()
    {
        Configuration = LoadConfiguration();
        ErpDriver = new ErpRealDriver(Configuration.ErpBaseUrl);
        TaxDriver = new TaxRealDriver(Configuration.TaxBaseUrl);
    }

    protected void SetUpShopApiDriver()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }
        ShopDriver = new ShopApiDriver(Configuration.ShopApiBaseUrl);
    }

    protected void SetUpShopUiDriver()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }
        ShopDriver = ShopUiDriver.CreateAsync(Configuration.ShopUiBaseUrl).Result;
    }

    protected virtual void TearDown()
    {
        ShopDriver?.DisposeAsync().AsTask().Wait();
        ErpDriver?.Dispose();
        TaxDriver?.Dispose();
    }
}
