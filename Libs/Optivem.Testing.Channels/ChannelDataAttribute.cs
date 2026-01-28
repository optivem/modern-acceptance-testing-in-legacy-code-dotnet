using System.Reflection;
using Xunit.Sdk;

namespace Optivem.Testing.Channels;

/// <summary>
/// Creates test cases for one or more channels, optionally combined with inline data, class data, or member data.
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
/// Combined with class data:
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [ChannelClassData(typeof(EmptyArgumentsProvider))]
/// public void Test(Channel channel, string value) { }
/// 
/// Combined with member data:
/// [Theory]
/// [ChannelData(ChannelType.UI, ChannelType.API)]
/// [ChannelMemberData(nameof(GetTestData))]
/// public void Test(Channel channel, string value, string message) { }
/// 
/// Generates: 2 channels � N data rows = 2N test cases.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    private static Type THEORY_ATTRIBUTE_TYPE = Type.GetType("Xunit.TheoryAttribute, xunit.core")!;
    private static Type FACT_ATTRIBUTE_TYPE = Type.GetType("Xunit.FactAttribute, xunit.core")!;
    private static readonly Type INLINE_DATA_ATTRIBUTE_TYPE = Type.GetType("Xunit.InlineDataAttribute, xunit.core")!;
    private static readonly Type CLASS_DATA_ATTRIBUTE_TYPE = Type.GetType("Xunit.ClassDataAttribute, xunit.core")!;
    private static readonly Type MEMBER_DATA_ATTRIBUTE_TYPE = Type.GetType("Xunit.MemberDataAttribute, xunit.core")!;

    public ChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));

        if (_channels.Length == 0)
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Check for correct Theory attribute usage
        ValidateTheoryAttributePresent(testMethod);
        
        // Check for incorrect usage of standard xUnit attributes
        ValidateNoStandardXUnitAttributes(testMethod);

        // Check for ChannelInlineData attributes
        var inlineDataAttributes = testMethod
            .GetCustomAttributes(typeof(ChannelInlineDataAttribute), false)
            .Cast<ChannelInlineDataAttribute>()
            .ToArray();

        // Check for ChannelClassData attribute
        var classDataAttribute = testMethod
            .GetCustomAttribute<ChannelClassDataAttribute>();

        // Check for ChannelMemberData attribute
        var memberDataAttribute = testMethod
            .GetCustomAttribute<ChannelMemberDataAttribute>();

        // If no inline data, class data, or member data, just return channels (simple mode)
        if (inlineDataAttributes.Length == 0 && classDataAttribute == null && memberDataAttribute == null)
        {
            foreach (var channel in _channels)
            {
                yield return new object[] { new Channel(channel) };
            }
        }
        // If ChannelInlineData is present
        else if (inlineDataAttributes.Length > 0)
        {
            // Create Cartesian product: channels � inline data (combinatorial mode)
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

            // Create Cartesian product: channels � class data
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
        // If ChannelMemberData is present
        else if (memberDataAttribute != null)
        {
            // Get data from the member
            var memberType = memberDataAttribute.MemberType ?? testMethod.DeclaringType;
            if (memberType == null)
            {
                throw new InvalidOperationException("Cannot determine the type containing the member.");
            }

            var member = memberType.GetMember(memberDataAttribute.MemberName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.FlattenHierarchy)
                .FirstOrDefault();

            if (member == null)
            {
                throw new InvalidOperationException(
                    $"Could not find member '{memberDataAttribute.MemberName}' on type '{memberType.Name}'");
            }

            object? memberValue = member switch
            {
                MethodInfo method => method.Invoke(null, null),
                PropertyInfo property => property.GetValue(null),
                FieldInfo field => field.GetValue(null),
                _ => throw new InvalidOperationException(
                    $"Member '{memberDataAttribute.MemberName}' must be a method, property, or field")
            };

            if (memberValue is not IEnumerable<object[]> dataProvider)
            {
                throw new InvalidOperationException(
                    $"Member '{memberDataAttribute.MemberName}' must return IEnumerable<object[]>");
            }

            // Create Cartesian product: channels � member data
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



    private static void ValidateTheoryAttributePresent(MethodInfo testMethod)
    {
        // Check for [Theory] attribute
        var theoryAttribute = testMethod.GetCustomAttribute(THEORY_ATTRIBUTE_TYPE);
        
        // Check for [Fact] attribute
        var factAttribute = testMethod.GetCustomAttribute(FACT_ATTRIBUTE_TYPE);
        
        if (theoryAttribute == null && factAttribute != null)
        {
            throw new InvalidOperationException(
                $"[ChannelData] requires [Theory], not [Fact]. " +
                $"Change [Fact] to [Theory]. " +
                $"Example: [Theory] [ChannelData(\"UI\", \"API\")] public void Test(Channel channel) {{ }}");
        }
        
        if (theoryAttribute == null && factAttribute == null)
        {
            throw new InvalidOperationException(
                $"[ChannelData] requires [Theory] attribute. " +
                $"Add [Theory] before [ChannelData]. " +
                $"Example: [Theory] [ChannelData(\"UI\", \"API\")] public void Test(Channel channel) {{ }}");
        }
    }



    private static void ValidateNoStandardXUnitAttributes(MethodInfo testMethod)
    {
        // Check for [InlineData]
        var inlineData = testMethod.GetCustomAttribute(INLINE_DATA_ATTRIBUTE_TYPE);
        if (inlineData != null)
        {
            throw new InvalidOperationException(
                $"Cannot use [InlineData] with [ChannelData]. " +
                $"Use [ChannelInlineData] instead. " +
                $"Example: [ChannelData(\"UI\", \"API\")] [ChannelInlineData(\"value1\", \"message1\")]");
        }

        // Check for [ClassData]
        var classData = testMethod.GetCustomAttribute(CLASS_DATA_ATTRIBUTE_TYPE);
        if (classData != null)
        {
            throw new InvalidOperationException(
                $"Cannot use [ClassData] with [ChannelData]. " +
                $"Use [ChannelClassData] instead. " +
                $"Example: [ChannelData(\"UI\", \"API\")] [ChannelClassData(typeof(YourDataProvider))]");
        }

        // Check for [MemberData]
        var memberData = testMethod.GetCustomAttribute(MEMBER_DATA_ATTRIBUTE_TYPE);
        if (memberData != null)
        {
            throw new InvalidOperationException(
                $"Cannot use [MemberData] with [ChannelData]. " +
                $"Use [ChannelMemberData] instead. " +
                $"Example: [ChannelData(\"UI\", \"API\")] [ChannelMemberData(nameof(YourMethod))]");
        }
    }
}
