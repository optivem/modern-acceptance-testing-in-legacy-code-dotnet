using Dsl.Gherkin;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V7;

/// <summary>
/// V7: Complete ScenarioDsl test infrastructure (with Browser lifecycle).
/// Most advanced version with full Playwright lifecycle management.
/// Recommended for new tests.
/// </summary>
public abstract class BaseScenarioDslTest : BaseConfigurableTest, IAsyncLifetime
{
    private SystemDsl _app = null!;
    protected ScenarioDsl Scenario { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        var configuration = LoadConfiguration();
        _app = new SystemDsl(configuration);
        Scenario = new ScenarioDsl(new Channel("Api"), _app);
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }
}
