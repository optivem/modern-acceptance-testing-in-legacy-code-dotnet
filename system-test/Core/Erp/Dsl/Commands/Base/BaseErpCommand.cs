using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> : BaseCommand<ErpApiDriver, TResponse, TVerification>
{
    protected BaseErpCommand(ErpApiDriver driver, Context context) 
        : base(driver, context)
    {
    }
}
