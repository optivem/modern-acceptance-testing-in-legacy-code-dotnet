using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels.Library;

/// <summary>
/// Creates a Cartesian product of channels with multiple inline data rows.
/// Follows xUnit's naming convention (similar to [InlineData]).
/// 
/// Example usage:
/// [Theory]
/// [CombinatorialChannelData(ChannelType.UI, ChannelType.API)]
/// [CombinatorialInlineData("", "Country must not be empty")]
/// [CombinatorialInlineData("   ", "Country must not be empty")]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// This generates 4 test cases (2 channels × 2 data rows).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CombinatorialChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public CombinatorialChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));

        if (_channels.Length == 0)
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Get all CombinatorialInlineData attributes
        var inlineDataAttributes = testMethod
            .GetCustomAttributes(typeof(CombinatorialInlineDataAttribute), false)
            .Cast<CombinatorialInlineDataAttribute>()
            .ToArray();

        if (inlineDataAttributes.Length == 0)
        {
            throw new InvalidOperationException(
                $"Method {testMethod.Name} must have at least one [CombinatorialInlineData] attribute");
        }

        // Create Cartesian product: channels × inline data
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

/// <summary>
/// Specifies inline test data for use with [CombinatorialChannelData].
/// Follows xUnit's [InlineData] naming convention.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CombinatorialInlineDataAttribute : Attribute
{
    public object[] Data { get; }

    /// <summary>
    /// Specifies test data parameters (excluding the channel parameter).
    /// </summary>
    /// <param name="data">Test data values</param>
    public CombinatorialInlineDataAttribute(params object[] data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}

