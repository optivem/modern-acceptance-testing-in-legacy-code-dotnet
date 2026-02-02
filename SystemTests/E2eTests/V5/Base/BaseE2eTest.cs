using Commons.Dsl;
using Optivem.EShop.SystemTest.Base.V5;

namespace Optivem.EShop.SystemTest.E2eTests.V5.Base;

public abstract class BaseE2eTest : BaseSystemDslTest
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
