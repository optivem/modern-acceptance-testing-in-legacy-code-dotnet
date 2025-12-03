using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Combines ChannelData with InlineData to create a Cartesian product of test cases.
/// Example: [ChannelInlineData(ChannelType.UI, ChannelType.API, "", "   ")]
/// Will generate: (UI, ""), (UI, "   "), (API, ""), (API, "   ")
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ChannelInlineDataAttribute : DataAttribute
{
    private readonly string[] _channels;
    private readonly object[] _data;

    /// <summary>
    /// Creates test cases for multiple channels with a single data value.
    /// </summary>
    public ChannelInlineDataAttribute(string channel1, string channel2, params object[] data)
    {
        _channels = new[] { channel1, channel2 };
        _data = data;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // If there's only one additional parameter after Channel, treat each data item as a separate test case
        var parameters = testMethod.GetParameters();
        var dataParameterCount = parameters.Length - 1; // Subtract 1 for the Channel parameter

        if (dataParameterCount == 1)
        {
            // Single parameter mode: (UI, ""), (UI, "   "), (API, ""), (API, "   ")
            foreach (var channel in _channels)
            {
                foreach (var dataItem in _data)
                {
                    yield return new object[] { new Channel(channel), dataItem };
                }
            }
        }
        else
        {
            // Multiple parameter mode: treat all _data as a single row of parameters
            // (UI, param1, param2, ...), (API, param1, param2, ...)
            foreach (var channel in _channels)
            {
                var row = new List<object> { new Channel(channel) };
                row.AddRange(_data);
                yield return row.ToArray();
            }
        }
    }
}

