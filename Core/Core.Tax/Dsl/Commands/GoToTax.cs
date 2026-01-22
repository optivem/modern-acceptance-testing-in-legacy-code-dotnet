using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    public GoToTax(ITaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override UseCaseResult<VoidValue, TaxErrorResponse, UseCaseContext, VoidVerification<UseCaseContext>, TaxErrorVerification> Execute()
    {
        var result = _driver.GoToTax();
        
        return new UseCaseResult<VoidValue, TaxErrorResponse, UseCaseContext, VoidVerification<UseCaseContext>, TaxErrorVerification>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx),
            (error, ctx) => new TaxErrorVerification(error, ctx));
    }
}
