using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands.Base;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> : BaseCommand<ErpApiDriver, TResponse, TVerification>
{
    protected BaseErpCommand(ErpApiDriver driver, TestContext context) 
        : base(driver, context)
    {
    }
}
