using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.Testing.Channels
{
    public class ChannelDataGenerator
    {
        public static IEnumerable<object[]> Generate(
            string[] channels, (string value, string message)[] testCases)
        {
            foreach (var channelType in channels)
            {
                foreach (var (value, message) in testCases)
                {
                    yield return new object[] { new Channel(channelType), value, message };
                }
            }
        }
    }
}
