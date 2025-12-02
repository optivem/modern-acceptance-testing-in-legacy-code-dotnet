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
        // No action
    }

    public override void After(MethodInfo methodUnderTest)
    {
        ChannelContext.Clear();
    }
}
