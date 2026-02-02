using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.E2eTests.Commons.Providers;

/// <summary>
/// Provides test arguments for empty quantity validation tests.
/// </summary>
public class EmptyArgumentsProvider : DataAttribute
{
    public override IEnumerable<object[]> GetData(System.Reflection.MethodInfo testMethod)
    {
        yield return new object[] { "" };        // Empty string
        yield return new object[] { "   " };     // Whitespace string
    }
}
