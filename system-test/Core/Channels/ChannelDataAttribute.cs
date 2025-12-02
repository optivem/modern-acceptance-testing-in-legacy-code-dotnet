using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Provides test data for channel-based parameterized tests.
/// This attribute enables running the same test across multiple channels (UI, API, etc.).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelDataAttribute : DataAttribute
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

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        foreach (var channel in _channels)
        {
            yield return new object[] { channel };
        }
    }
}
