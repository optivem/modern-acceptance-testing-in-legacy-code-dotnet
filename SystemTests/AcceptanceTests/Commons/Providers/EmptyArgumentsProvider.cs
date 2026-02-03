using System.Collections;

namespace Optivem.EShop.SystemTest.AcceptanceTests.Commons.Providers;

/// <summary>
/// Provides test arguments for empty quantity validation tests.
/// </summary>
public class EmptyArgumentsProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "" };        // Empty string
        yield return new object[] { "   " };     // Whitespace string
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
