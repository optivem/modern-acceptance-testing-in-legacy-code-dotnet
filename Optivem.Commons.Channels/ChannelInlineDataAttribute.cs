namespace Optivem.Commons.Channels;

/// <summary>
/// Specifies inline test data for use with [ChannelData].
/// When combined with [ChannelData], creates a Cartesian product of channels × data rows.
/// Follows xUnit's [InlineData] naming convention.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ChannelInlineDataAttribute : Attribute
{
    public object[] Data { get; }

    /// <summary>
    /// Specifies test data parameters (excluding the channel parameter).
    /// </summary>
    /// <param name="data">Test data values</param>
    public ChannelInlineDataAttribute(params object[] data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}
