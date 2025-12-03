using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels;

/// <summary>
/// Custom data discoverer that combines ChannelData and InlineData attributes
/// to create a Cartesian product of test cases.
/// </summary>
public class CompositeDataDiscoverer : IDataDiscoverer
{
    public IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
    {
        // Use reflection to get the actual MethodInfo from IMethodInfo
        var method = GetMethodInfo(testMethod);
        if (method == null) yield break;

        // Get ChannelData attribute
        var channelDataAttr = method.GetCustomAttributes(typeof(ChannelDataAttribute), false).FirstOrDefault() as ChannelDataAttribute;
        if (channelDataAttr == null) yield break;

        // Get InlineData attributes
        var inlineDataAttrs = method.GetCustomAttributes(typeof(InlineDataAttribute), false).Cast<InlineDataAttribute>().ToArray();
        if (!inlineDataAttrs.Any()) yield break;

        // Get channels from ChannelDataAttribute using reflection
        var channelsField = typeof(ChannelDataAttribute).GetField("_channels", BindingFlags.NonPublic | BindingFlags.Instance);
        var channels = channelsField?.GetValue(channelDataAttr) as string[] ?? Array.Empty<string>();

        // Create Cartesian product
        foreach (var channel in channels)
        {
            foreach (var inlineDataAttr in inlineDataAttrs)
            {
                var inlineData = inlineDataAttr.GetData(method).FirstOrDefault();
                if (inlineData == null) continue;

                // Combine: [Channel] + [InlineData params]
                var combinedData = new List<object> { new Channel(channel) };
                combinedData.AddRange(inlineData);

                yield return combinedData.ToArray();
            }
        }
    }

    private MethodInfo? GetMethodInfo(IMethodInfo methodInfo)
    {
        var typeInfo = methodInfo.Type as IReflectionTypeInfo;
        if (typeInfo == null) return null;

        var type = typeInfo.Type;
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        return methods.FirstOrDefault(m => m.Name == methodInfo.Name);
    }

    public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) => true;
}

/// <summary>
/// Marker attribute that combines ChannelData and InlineData into a Cartesian product.
/// 
/// IMPORTANT: When using [CompositeData], the [ChannelData] and [InlineData] attributes
/// are ONLY used as data sources for the composite - they won't generate their own test cases.
/// 
/// Usage:
/// [Theory]
/// [CompositeData]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [InlineData("", "Expected error")]
/// [InlineData("   ", "Expected error")]
/// public void MyTest(Channel channel, string input, string expectedError) 
/// {
///     // Generates 4 test cases: 2 channels × 2 inline data rows
/// }
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[DataDiscoverer("Optivem.EShop.SystemTest.Core.Channels.CompositeDataDiscoverer", "Optivem.EShop.SystemTest")]
public class CompositeDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Actual data generation is handled by CompositeDataDiscoverer
        yield break;
    }
}


