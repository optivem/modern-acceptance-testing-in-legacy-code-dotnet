using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.SmokeTests;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ShopUiSmokeTest : BaseShopSmokeTest
{
    protected override IShopDriver CreateDriver()
    {
        return new ShopUiDriver(TestConfiguration.ShopUiBaseUrl);
    }
}

