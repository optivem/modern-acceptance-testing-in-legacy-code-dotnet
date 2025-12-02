using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Attribute that automatically sets up channel context before test execution.
/// Works in conjunction with [ChannelData] attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelSetupAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        // Channel context will be set by the test class's SetupChannel method
        // This attribute ensures proper ordering
    }

    public override void After(MethodInfo methodUnderTest)
    {
        // Cleanup handled by Dispose
    }
}
