using Xunit;

namespace Optivem.Testing;

/// <summary>
/// Trait attribute to mark tests that require isolation from other tests.
/// 
/// <para>Use this attribute for tests that:</para>
/// <list type="bullet">
///   <item><description>Modify shared state (e.g., deleting all orders)</description></item>
///   <item><description>Depend on specific time values (TimeAttribute tests)</description></item>
///   <item><description>Have side effects that could affect other tests</description></item>
///   <item><description>Need exclusive access to resources</description></item>
/// </list>
/// 
/// <para><b>Filtering Tests</b></para>
/// 
/// <para><b>Run ONLY isolated tests:</b></para>
/// <code>
/// dotnet test --filter "Category=isolated"
/// </code>
/// 
/// <para><b>Run all tests EXCEPT isolated tests:</b></para>
/// <code>
/// dotnet test --filter "Category!=isolated"
/// </code>
/// 
/// <para><b>IDE Support:</b></para>
/// <para>Visual Studio and other test runners will recognize this trait automatically.</para>
/// </summary>
[TraitAttribute("Category", "isolated")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class IsolatedAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IsolatedAttribute"/> class.
    /// </summary>
    public IsolatedAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsolatedAttribute"/> class with a reason.
    /// </summary>
    /// <param name="reason">Optional reason why this test needs isolation.</param>
    public IsolatedAttribute(string reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// Gets the reason why this test needs isolation.
    /// </summary>
    public string? Reason { get; }
}
