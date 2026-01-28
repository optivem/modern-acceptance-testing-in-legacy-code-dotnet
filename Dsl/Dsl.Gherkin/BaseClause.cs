using Optivem.Testing.Channels;

namespace Dsl.Gherkin
{
    public class BaseClause
    {
        internal Channel Channel { get; }

        public BaseClause(Channel channel)
        {
            Channel = channel;
        }
    }
}
