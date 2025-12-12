using Optivem.Testing.Channels;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Channels;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Api;

namespace Optivem.EShop.SystemTest.Core;

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

    public static ErpApiDriver CreateErpApiDriver()
    {
        return new ErpApiDriver(TestConfiguration.GetErpApiBaseUrl());
    }

    public static TaxApiDriver CreateTaxApiDriver()
    {
        return new TaxApiDriver(TestConfiguration.GetTaxApiBaseUrl());
    }
}
