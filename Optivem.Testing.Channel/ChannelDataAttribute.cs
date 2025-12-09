using System.Reflection;
using Xunit.Sdk;

namespace Optivem.Testing.Channel;

/// <summary>
/// Creates test cases for one or more channels, optionally combined with inline data.
/// 
/// Simple usage (generates one test per channel):
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// public void Test(Channel channel) { }
/// 
/// Combined with inline data (generates Cartesian product):
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [ChannelInlineData("", "Country must not be empty")]
/// [ChannelInlineData("   ", "Country must not be empty")]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// Generates: 2 channels × 2 data rows = 4 test cases.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public ChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));

        if (_channels.Length == 0)
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Check for CombinatorialInlineData attributes
        var inlineDataAttributes = testMethod
            .GetCustomAttributes(typeof(ChannelInlineDataAttribute), false)
            .Cast<ChannelInlineDataAttribute>()
            .ToArray();

        // If no inline data, just return channels (simple mode)
        if (inlineDataAttributes.Length == 0)
        {
            foreach (var channel in _channels)
            {
                yield return new object[] { new Channel(channel) };
            }
        }
        else
        {
            // Create Cartesian product: channels × inline data (combinatorial mode)
            foreach (var channel in _channels)
            {
                foreach (var inlineDataAttr in inlineDataAttributes)
                {
                    var testCase = new List<object> { new Channel(channel) };
                    testCase.AddRange(inlineDataAttr.Data);
                    yield return testCase.ToArray();
                }
            }
        }
    }
}



