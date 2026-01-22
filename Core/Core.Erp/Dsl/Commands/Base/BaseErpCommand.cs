using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> 
    : BaseUseCase<ErpDriver, UseCaseContext, TResponse, ErpErrorResponse, TVerification, ErpErrorVerification>
{
    protected BaseErpCommand(ErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
