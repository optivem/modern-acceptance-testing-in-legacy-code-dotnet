using Optivem.EShop.SystemTest.Core.Channels;
using Optivem.Testing.Channels;
using System.Collections;
using System.Collections.Generic;

namespace Optivem.EShop.SystemTest.E2eTests.Providers;

public class EmptyArgumentsProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var channels = new[] { ChannelType.UI, ChannelType.API };
        var testCases = new[]
        {
            "",
            "   "
        };

        foreach (var channelType in channels)
        {
            foreach (var quantity in testCases)
            {
                yield return new object[] { new Channel(channelType), quantity };
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


