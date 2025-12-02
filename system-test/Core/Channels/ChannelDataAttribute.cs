using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Provides channel test data for parameterized tests.
/// This is a generic attribute that can be used with any channel type.
/// </summary>
public class ChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public ChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        foreach (var channel in _channels)
        {
            yield return new object[] { new Channel(channel) };
        }
    }
}

/// <summary>
/// Represents a test channel (e.g., UI, API).
/// This is a generic class that holds the channel name and can be extended with factory methods.
/// </summary>
public class Channel
{
    private readonly string _value;

    public Channel(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value => _value;

    public override string ToString() => _value;
}
