using Optivem.EShop.SystemTest.Core;
using Optivem.Testing.Channels;

namespace Dsl.Gherkin
{
    public class ScenarioDslFactory
    {
        private readonly SystemDsl _app;

        public ScenarioDslFactory(SystemDsl app)
        {
            _app = app;
        }

        public ScenarioDsl Create(Channel channel) { return new ScenarioDsl(channel, _app); }
    }
}
