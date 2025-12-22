using System.Collections;
using System.Collections.Generic;

namespace E2eTests.Providers;

/// <summary>
/// Provides test data for empty value scenarios.
/// Returns data rows (excluding channel) to be combined with [ChannelData].
/// </summary>
public class EmptyArgumentsProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "" };
        yield return new object[] { "   " };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
