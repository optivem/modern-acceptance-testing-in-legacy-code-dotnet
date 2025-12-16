using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<VoidValue, VoidResponseVerification<UseCaseContext>>
{
    public GoToTax(TaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override TaxUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToTax();
        return new TaxUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidResponseVerification<UseCaseContext>(response, ctx));
    }
}
