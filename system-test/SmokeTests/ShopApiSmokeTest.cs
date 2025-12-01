using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopApiSmokeTest : BaseShopSmokeTest
{
    protected override IShopDriver CreateShopDriver()
    {
        return DriverFactory.CreateShopApiDriver();
    }
}
