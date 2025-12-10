using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> : BaseCommand<ErpApiDriver, TResponse, TVerification>
{
    protected BaseErpCommand(ErpApiDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
