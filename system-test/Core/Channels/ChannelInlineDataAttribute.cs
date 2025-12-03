using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Combines ChannelData with InlineData to create a Cartesian product of test cases.
/// 
/// Examples:
/// - Single channel: [ChannelInlineData(new[] { ChannelType.UI }, "", "   ")]
/// - Two channels: [ChannelInlineData(new[] { ChannelType.UI, ChannelType.API }, "", "   ")]
/// - Three channels: [ChannelInlineData(new[] { ChannelType.UI, ChannelType.API, ChannelType.Mobile }, "", "   ")]
/// 
/// For two channels, generates: (UI, ""), (UI, "   "), (API, ""), (API, "   ")
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ChannelInlineDataAttribute : DataAttribute
{
    private readonly string[] _channels;
    private readonly object[] _data;

    /// <summary>
    /// Creates test cases for multiple channels with data parameters.
    /// </summary>
    /// <param name="channels">Array of channel types (e.g., new[] { ChannelType.UI, ChannelType.API })</param>
    /// <param name="data">Data parameters for the test</param>
    public ChannelInlineDataAttribute(string[] channels, params object[] data)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
        _data = data ?? throw new ArgumentNullException(nameof(data));
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


