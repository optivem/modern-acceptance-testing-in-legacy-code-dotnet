using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.EShop.SystemTest.Core.Channels.Library;

namespace Optivem.EShop.SystemTest.Core.Drivers;

public static class DriverFactory
{
    public static IShopDriver CreateShopDriver()
    {
        var channel = ChannelContext.Get();
        return channel switch
        {
            ChannelType.UI => new ShopUiDriver(TestConfiguration.GetShopUiBaseUrl()),
            ChannelType.API => new ShopApiDriver(TestConfiguration.GetShopApiBaseUrl()),
            _ => throw new InvalidOperationException($"Unknown channel: {channel}")
        };
    }

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
