using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Erp.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Dsl;
using Optivem.EShop.SystemTest.Core.Tax.Dsl;
using Optivem.Testing.Channels;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest;

public class SystemDslFactory
{
    public static SystemDsl Create()
    {
        var configuaration = SystemConfigurationLoader.Load();
        return new SystemDsl(configuaration);
    }
}
