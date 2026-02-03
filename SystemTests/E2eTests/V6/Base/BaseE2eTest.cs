using Commons.Dsl;
using Optivem.EShop.SystemTest.Base.V6;

namespace Optivem.EShop.SystemTest.E2eTests.V6.Base;

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
