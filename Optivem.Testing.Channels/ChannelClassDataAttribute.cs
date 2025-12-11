using System.Collections;

namespace Optivem.Testing.Channels;

/// <summary>
/// Specifies a class-based test data provider for use with [ChannelData].
/// When combined with [ChannelData], creates a Cartesian product of channels × data from the provider class.
/// Follows xUnit's [ClassData] naming convention.
/// 
/// Example:
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [ChannelClassData(typeof(EmptyArgumentsProvider))]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// The provider class must implement IEnumerable&lt;object[]&gt; and return data rows (excluding the channel parameter).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelClassDataAttribute : Attribute
{
    public Type ProviderType { get; }

    /// <summary>
    /// Specifies a class that provides test data (excluding the channel parameter).
    /// </summary>
    /// <param name="providerType">Type that implements IEnumerable&lt;object[]&gt;</param>
    public ChannelClassDataAttribute(Type providerType)
    {
        ProviderType = providerType ?? throw new ArgumentNullException(nameof(providerType));

        if (!typeof(IEnumerable).IsAssignableFrom(providerType))
        {
            throw new ArgumentException(
                $"Type {providerType.Name} must implement IEnumerable<object[]>",
                nameof(providerType));
        }
    }
}
