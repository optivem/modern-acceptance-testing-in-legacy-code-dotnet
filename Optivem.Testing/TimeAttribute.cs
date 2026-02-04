using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Optivem.Testing;

/// <summary>
/// Trait attribute to mark tests that depend on specific time values.
/// These tests require isolation from other tests and controlled time setup.
/// 
/// <para>This attribute automatically includes <see cref="IsolatedAttribute"/> since time-dependent
/// tests require isolation from other tests.</para>
/// 
/// <para><b>Example usage:</b></para>
/// <code>
/// [Time("2024-01-15T17:30:00Z")]
/// public void DiscountRateShouldBe15percentWhenTimeAfter5pm()
/// {
///     // Test implementation
/// }
/// </code>
/// 
/// <para><b>Filtering Tests</b></para>
/// 
/// <para><b>Run ONLY time-dependent tests:</b></para>
/// <code>
/// dotnet test --filter "Category=time"
/// </code>
/// 
/// <para><b>Run all tests EXCEPT time-dependent tests:</b></para>
/// <code>
/// dotnet test --filter "Category!=time"
/// </code>
/// 
/// <para><b>IDE Support:</b></para>
/// <para>Visual Studio and other test runners will recognize this trait automatically.</para>
/// </summary>
[TraitDiscoverer("Optivem.Testing.TimeTraitDiscoverer", "Optivem.Testing")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class TimeAttribute : Attribute, ITraitAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeAttribute"/> class.
    /// </summary>
    public TimeAttribute()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeAttribute"/> class with a specific time value.
    /// </summary>
    /// <param name="value">The specific time value for this test (ISO-8601 format).
    /// Used for documentation and potential future automation.</param>
    public TimeAttribute(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the specific time value for this test (ISO-8601 format).
    /// </summary>
    public string Value { get; }
}

/// <summary>
/// Trait discoverer for TimeAttribute that adds both 'time' and 'isolated' traits.
/// </summary>
public class TimeTraitDiscoverer : ITraitDiscoverer
{
    /// <summary>
    /// Gets the traits for a test decorated with TimeAttribute.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        yield return new KeyValuePair<string, string>("Category", "time");
        yield return new KeyValuePair<string, string>("Category", "isolated");
    }
}
