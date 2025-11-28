using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class ShopUiSmokeTest : BaseShopSmokeTest
{
    protected override IShopDriver CreateDriver()
    {
        return DriverFactory.CreateShopUiDriver();
    }
}
