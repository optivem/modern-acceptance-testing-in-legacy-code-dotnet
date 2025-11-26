using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public class UiE2eTest : BaseE2eTest
{
    protected override IShopDriver CreateDriver()
    {
        return DriverFactory.CreateShopUiDriver();
    }
}
