using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification> 
    : BaseUseCase<ITaxDriver, TResponse, TaxErrorResponse, TVerification, TaxErrorVerification>
{
    protected BaseTaxCommand(ITaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
