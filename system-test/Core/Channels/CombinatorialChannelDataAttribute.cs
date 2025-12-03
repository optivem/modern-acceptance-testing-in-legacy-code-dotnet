using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Creates a Cartesian product of channels with multiple data rows.
/// 
/// Example usage:
/// [Theory]
/// [CombinatorialChannelData(ChannelType.UI, ChannelType.API)]
/// [CombinatorialDataRow("", "Country must not be empty")]
/// [CombinatorialDataRow("   ", "Country must not be empty")]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// This generates: 
/// - (UI, "", "Country must not be empty")
/// - (UI, "   ", "Country must not be empty")
/// - (API, "", "Country must not be empty")
/// - (API, "   ", "Country must not be empty")
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CombinatorialChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    /// <summary>
    /// Creates test cases for multiple channels. Use with [CombinatorialDataRow] attributes.
    /// </summary>
    /// <param name="channels">Channel types (e.g., ChannelType.UI, ChannelType.API)</param>
    public CombinatorialChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));

        if (_channels.Length == 0)
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Get all CombinatorialDataRow attributes
        var dataRowAttributes = testMethod
            .GetCustomAttributes(typeof(CombinatorialDataRowAttribute), false)
            .Cast<CombinatorialDataRowAttribute>()
            .ToArray();

        if (dataRowAttributes.Length == 0)
        {
            throw new InvalidOperationException(
                $"Method {testMethod.Name} must have at least one [CombinatorialDataRow] attribute");
        }

        // Create Cartesian product: channels × data rows
        foreach (var channel in _channels)
        {
            foreach (var dataRowAttr in dataRowAttributes)
            {
                var testCase = new List<object> { new Channel(channel) };
                testCase.AddRange(dataRowAttr.Data);
                yield return testCase.ToArray();
            }
        }
    }
}

/// <summary>
/// Specifies a data row for use with [CombinatorialChannelData].
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CombinatorialDataRowAttribute : Attribute
{
    public object[] Data { get; }

    /// <summary>
    /// Specifies test data parameters (excluding the channel parameter).
    /// </summary>
    /// <param name="data">Test data values</param>
    public CombinatorialDataRowAttribute(params object[] data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}

