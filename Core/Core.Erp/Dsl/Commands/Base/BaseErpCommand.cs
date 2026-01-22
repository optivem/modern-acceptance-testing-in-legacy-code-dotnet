using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> 
    : BaseUseCase<IErpDriver, UseCaseContext, TResponse, ErpErrorResponse, TVerification, ErpErrorVerification>
{
    protected BaseErpCommand(IErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
