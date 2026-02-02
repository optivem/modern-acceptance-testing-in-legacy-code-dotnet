using Dsl.Gherkin;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.Base;

[Obsolete("Use BaseScenarioTest or BaseSystemDslTest instead")]
public abstract class BaseSystemTest : BaseConfigurableTest, IAsyncDisposable
{
    protected SystemDsl _app { get; private set; } = null!;
    private readonly ScenarioDslFactory _scenarioFactory;

    protected BaseSystemTest()
    {
        var configuration = LoadConfiguration();
        _app = new SystemDsl(configuration);
        _scenarioFactory = new ScenarioDslFactory(_app);
    }

    protected ScenarioDsl Scenario(Channel channel) => _scenarioFactory.Create(channel);

    public async ValueTask DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }
}
