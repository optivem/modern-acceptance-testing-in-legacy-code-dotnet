using Optivem.EShop.SystemTest.Core;

namespace SmokeTests;

public static class SystemDslFactory
{
    public static SystemDsl Create()
    {
        var configuration = SystemConfigurationLoader.Load();
        return new SystemDsl(configuration);
    }
}
