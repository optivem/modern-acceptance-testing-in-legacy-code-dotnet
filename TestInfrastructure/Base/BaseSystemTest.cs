using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Xunit;

namespace Optivem.EShop.SystemTest.Base;

public abstract class BaseSystemTest : IDisposable
{
    protected SystemDsl App { get; }
    protected ScenarioDsl Scenario { get; }

    protected BaseSystemTest()
    {
        var configuration = SystemConfigurationLoader.Load();
        App = new SystemDsl(configuration);
        Scenario = new ScenarioDsl(App);
    }

    public void Dispose()
    {
        App?.Dispose();
        GC.SuppressFinalize(this);
    }
}
