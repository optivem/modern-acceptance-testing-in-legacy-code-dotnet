using Dsl.Gherkin;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.Base;

public abstract class BaseSystemTest : IDisposable
{
    private readonly ScenarioDslFactory _scenarioFactory;

    protected SystemDsl App { get; }

    protected BaseSystemTest()
    {
        var externalSystemMode = ExternalSystemMode.Real; // TODO: VJ: Make dynamic
        var configuration = SystemConfigurationLoader.Load(externalSystemMode);
        App = new SystemDsl(configuration);
        _scenarioFactory = new ScenarioDslFactory(App);
    }

    public void Dispose()
    {
        App?.Dispose();
        GC.SuppressFinalize(this);
    }

    protected ScenarioDsl Scenario(Channel channel) { return _scenarioFactory.Create(channel); }
}
