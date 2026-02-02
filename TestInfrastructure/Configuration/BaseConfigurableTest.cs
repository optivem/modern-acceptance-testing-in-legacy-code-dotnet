using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.Configuration;

public abstract class BaseConfigurableTest
{
    protected virtual Environment? GetFixedEnvironment()
    {
        return null;
    }

    protected virtual ExternalSystemMode? GetFixedExternalSystemMode()
    {
        return null;
    }

    protected SystemConfiguration LoadConfiguration()
    {
        var fixedEnvironment = GetFixedEnvironment();
        var fixedExternalSystemMode = GetFixedExternalSystemMode();

        var environment = PropertyLoader.GetEnvironment(fixedEnvironment);
        var externalSystemMode = PropertyLoader.GetExternalSystemMode(fixedExternalSystemMode);

        return SystemConfigurationLoader.Load(environment, externalSystemMode);
    }
}
