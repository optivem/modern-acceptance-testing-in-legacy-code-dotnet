using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.SmokeTests;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ShopApiSmokeTest : BaseShopSmokeTest
{
    protected override IShopDriver CreateDriver()
    {
        return new ShopApiDriver(TestConfiguration.ShopApiBaseUrl);
    }
}
