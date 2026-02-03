using Commons.Dsl;
using Optivem.EShop.SystemTest.Base.V7;

namespace Optivem.EShop.SystemTest.E2eTests.V7.Base;

public abstract class BaseE2eTest : BaseScenarioDslTest
{
    protected override Configuration.Environment? GetFixedEnvironment()
    {
        return Configuration.Environment.Local;
    }

    protected override ExternalSystemMode? GetFixedExternalSystemMode()
    {
        return ExternalSystemMode.Real;
    }
}
