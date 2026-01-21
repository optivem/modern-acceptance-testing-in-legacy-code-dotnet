using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Util;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    public GoToTax(TaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override TaxUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToTax();
        return new TaxUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
