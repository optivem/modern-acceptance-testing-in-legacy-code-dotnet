using Commons.Dsl;
using Optivem.EShop.SystemTest.Base.V7;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;

public abstract class BaseAcceptanceTest : BaseScenarioDslTest
{
    protected override ExternalSystemMode? GetFixedExternalSystemMode()
    {
        return ExternalSystemMode.Stub;
    }

    protected override Configuration.Environment? GetFixedEnvironment()
    {
        return Configuration.Environment.Local;
    }
}
