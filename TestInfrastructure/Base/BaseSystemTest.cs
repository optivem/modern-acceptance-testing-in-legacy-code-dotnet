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
    protected SystemDsl App { get; private set; } = null!;
    private readonly ScenarioDslFactory _scenarioFactory;

    protected BaseSystemTest()
    {
        var configuration = LoadConfiguration();
        App = new SystemDsl(configuration);
        _scenarioFactory = new ScenarioDslFactory(App);
    }

    protected ScenarioDsl Scenario(Channel channel) => _scenarioFactory.Create(channel);

    public async ValueTask DisposeAsync()
    {
        if (App != null)
            await App.DisposeAsync();
    }
}
