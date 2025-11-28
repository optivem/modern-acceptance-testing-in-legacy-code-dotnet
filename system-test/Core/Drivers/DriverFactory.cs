using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;

namespace Optivem.EShop.SystemTest.Core.Drivers;

public static class DriverFactory
{
    public static ShopUiDriver CreateShopUiDriver()
    {
        return new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl());
    }

    public static ShopApiDriver CreateShopApiDriver()
    {
        return new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl());
    }

    public static ErpApiDriver CreateErpApiDriver()
    {
        return new ErpApiDriver(TestConfiguration.GetErpApiBaseUrl());
    }

    public static TaxApiDriver CreateTaxApiDriver()
    {
        return new TaxApiDriver(TestConfiguration.GetTaxApiBaseUrl());
    }
}
