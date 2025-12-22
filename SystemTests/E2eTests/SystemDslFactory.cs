using Optivem.EShop.SystemTest.Core;

namespace E2eTests;

public static class SystemDslFactory
{
    public static SystemDsl Create()
    {
        var configuration = SystemConfigurationLoader.Load();
        return new SystemDsl(configuration);
    }
}
