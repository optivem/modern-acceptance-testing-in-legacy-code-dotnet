using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.External;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;

public static class DriverFactory
{
    public static ShopUiDriver CreateShopUiDriver()
    {
        return new ShopUiDriver(TestConfiguration.ShopUiBaseUrl);
    }

    public static ShopApiDriver CreateShopApiDriver()
    {
        return new ShopApiDriver(TestConfiguration.ShopApiBaseUrl);
    }

    public static ErpApiDriver CreateErpApiDriver()
    {
        return new ErpApiDriver(TestConfiguration.ErpApiBaseUrl);
    }

    public static TaxApiDriver CreateTaxApiDriver()
    {
        return new TaxApiDriver(TestConfiguration.TaxApiBaseUrl);
    }
}
