namespace Optivem.Testing.Channels;

/// <summary>
/// Specifies a member-based test data provider for use with [ChannelData].
/// When combined with [ChannelData], creates a Cartesian product of channels � data from the member.
/// Follows xUnit's [MemberData] naming convention.
/// 
/// Example:
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [ChannelMemberData(nameof(GetTestData))]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// The member must return IEnumerable&lt;object[]&gt; with data rows (excluding the channel parameter).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelMemberDataAttribute : Attribute
{
    public string MemberName { get; }
    public Type? MemberType { get; }

    /// <summary>
    /// Specifies a static member (method, property, or field) that provides test data (excluding the channel parameter).
    /// </summary>
    /// <param name="memberName">Name of the member that returns IEnumerable&lt;object[]&gt;</param>
    public ChannelMemberDataAttribute(string memberName)
    {
        MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
    }

    /// <summary>
    /// Specifies a static member from another type that provides test data (excluding the channel parameter).
    /// </summary>
    /// <param name="memberName">Name of the member that returns IEnumerable&lt;object[]&gt;</param>
    /// <param name="memberType">Type containing the member</param>
    public ChannelMemberDataAttribute(string memberName, Type memberType)
        : this(memberName)
    {
        MemberType = memberType;
    }
}
