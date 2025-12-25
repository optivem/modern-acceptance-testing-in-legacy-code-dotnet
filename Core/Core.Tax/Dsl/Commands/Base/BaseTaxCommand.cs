using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification> 
    : BaseUseCase<TaxDriver, UseCaseContext, TResponse, Error, TVerification, ErrorFailureVerification>
{
    protected BaseTaxCommand(TaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
