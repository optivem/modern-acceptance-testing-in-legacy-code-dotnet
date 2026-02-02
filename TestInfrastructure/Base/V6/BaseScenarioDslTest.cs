using Dsl.Gherkin;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V6;

public abstract class BaseScenarioDslTest : BaseConfigurableTest, IAsyncLifetime
{
    private SystemDsl _app = null!;
    protected ScenarioDsl Scenario { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        _app = new SystemDsl(configuration);
        Scenario = new ScenarioDsl(new Optivem.Testing.Channel("Api"), _app);
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }
}
