using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.EShop.SystemTest.E2eTests;

public class UiE2eTest : BaseE2eTest
{
    protected override IShopDriver CreateShopDriver()
    {
        return DriverFactory.CreateShopUiDriver();
    }
}
