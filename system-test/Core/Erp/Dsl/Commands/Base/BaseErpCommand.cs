using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification> 
    : BaseUseCase<ErpDriver, UseCaseContext, TResponse, Error, TVerification, ErrorFailureVerification>
{
    protected BaseErpCommand(ErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
