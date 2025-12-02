using System.Reflection;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Attribute that automatically initializes the shopDriver field based on the channel parameter.
/// Place this on test methods using [ChannelTestData].
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelSetupAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        // Setup will be done via the ChannelTestCase parameter
    }

    public override void After(MethodInfo methodUnderTest)
    {
        // Cleanup will be done by the test class Dispose
    }
}

/// <summary>
/// Base interface for channel-based tests that provides the shopDriver field.
/// </summary>
public interface IChannelTest
{
    IShopDriver? ShopDriver { get; set; }
}
