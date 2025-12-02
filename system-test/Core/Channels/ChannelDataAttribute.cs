using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Provides test data for channel-based parameterized tests and automatically sets up channel context.
/// This attribute enables running the same test across multiple channels (UI, API, etc.).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelDataAttribute : BeforeAfterTestAttribute, ITraitAttribute
{
    private readonly string[] _channels;

    public ChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
        if (_channels.Length == 0)
        {
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
        }
    }

    public string[] Channels => _channels;

    public override void Before(MethodInfo methodUnderTest)
    {
        // Unfortunately, BeforeAfterTestAttribute doesn't provide access to:
        // - The test instance (e.g., ShopSmokeTest object)
        // - The runtime parameter values (e.g., the specific ChannelTestCase)
        // This is a limitation of xUnit's design compared to JUnit's @BeforeEach
    }

    public override void After(MethodInfo methodUnderTest)
    {
        ChannelContext.Clear();
    }
}
