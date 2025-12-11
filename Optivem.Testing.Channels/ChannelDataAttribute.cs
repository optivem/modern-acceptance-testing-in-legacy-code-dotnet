using System.Reflection;
using Xunit.Sdk;

namespace Optivem.Testing.Channels;

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
/// Generates: 2 channels � 2 data rows = 4 test cases.
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
        // Check for ChannelInlineData attributes
        var inlineDataAttributes = testMethod
            .GetCustomAttributes(typeof(ChannelInlineDataAttribute), false)
            .Cast<ChannelInlineDataAttribute>()
            .ToArray();

        // Check for ChannelClassData attribute
        var classDataAttribute = testMethod
            .GetCustomAttribute<ChannelClassDataAttribute>();

        // If no inline data or class data, just return channels (simple mode)
        if (inlineDataAttributes.Length == 0 && classDataAttribute == null)
        {
            foreach (var channel in _channels)
            {
                yield return new object[] { new Channel(channel) };
            }
        }
        // If ChannelInlineData is present
        else if (inlineDataAttributes.Length > 0)
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
        // If ChannelClassData is present
        else if (classDataAttribute != null)
        {
            // Get data from the provider class
            var providerInstance = Activator.CreateInstance(classDataAttribute.ProviderType);
            if (providerInstance is not IEnumerable<object[]> dataProvider)
            {
                throw new InvalidOperationException(
                    $"Type {classDataAttribute.ProviderType.Name} must implement IEnumerable<object[]>");
            }

            // Create Cartesian product: channels × class data
            foreach (var channel in _channels)
            {
                foreach (var dataRow in dataProvider)
                {
                    var testCase = new List<object> { new Channel(channel) };
                    testCase.AddRange(dataRow);
                    yield return testCase.ToArray();
                }
            }
        }
    }
}



