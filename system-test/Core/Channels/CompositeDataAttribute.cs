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
        // Get all attributes on the test method
        var allAttributes = testMethod.GetCustomAttributes(typeof(DataAttribute));
        
        var channelDataAttr = allAttributes.FirstOrDefault(a => a.GetType().Name == "ChannelDataAttribute");
        var inlineDataAttrs = allAttributes.Where(a => a.GetType().Name == "InlineDataAttribute").ToList();

        if (channelDataAttr == null || !inlineDataAttrs.Any())
        {
            yield break;
        }

        // Get channels
        var channels = channelDataAttr.GetConstructorArguments().FirstOrDefault() as string[] ?? Array.Empty<string>();

        // Get inline data rows
        foreach (var inlineAttr in inlineDataAttrs)
        {
            var inlineData = inlineAttr.GetConstructorArguments().FirstOrDefault() as object[] ?? Array.Empty<object>();

            // Create Cartesian product: each channel combined with this inline data row
            foreach (var channel in channels)
            {
                var combinedData = new List<object> { new Channel(channel) };
                combinedData.AddRange(inlineData);
                yield return combinedData.ToArray();
            }
        }
    }

    public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) => true;
}

/// <summary>
/// Marker attribute that tells xUnit to use the CompositeDataDiscoverer
/// to combine ChannelData and InlineData attributes.
/// 
/// Usage:
/// [Theory]
/// [CompositeData]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [InlineData("", "Expected error")]
/// [InlineData("   ", "Expected error")]
/// public void MyTest(Channel channel, string input, string expectedError) { }
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[DataDiscoverer("Optivem.EShop.SystemTest.Core.Channels.CompositeDataDiscoverer", "Optivem.EShop.SystemTest")]
public class CompositeDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // This is handled by the CompositeDataDiscoverer
        yield break;
    }
}
