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
    protected SystemConfiguration? Configuration;

    protected IShopDriver? ShopDriver;
    protected ErpRealDriver? ErpDriver;
    protected TaxRealDriver? TaxDriver;

    protected void SetUpConfiguration()
    {
        Configuration = LoadConfiguration();
    }

    protected void SetUpShopUiDriver()
    {
        ShopDriver = ShopUiDriver.CreateAsync(Configuration!.ShopUiBaseUrl).Result;
    }

    protected void SetUpShopApiDriver()
    {
        ShopDriver = new ShopApiDriver(Configuration!.ShopApiBaseUrl);
    }

    protected void SetUpExternalDrivers()
    {
        ErpDriver = new ErpRealDriver(Configuration!.ErpBaseUrl);
        TaxDriver = new TaxRealDriver(Configuration!.TaxBaseUrl);
    }

    protected virtual void TearDown()
    {
        ShopDriver?.DisposeAsync().AsTask().Wait();
        ErpDriver?.Dispose();
        TaxDriver?.Dispose();
    }
}
