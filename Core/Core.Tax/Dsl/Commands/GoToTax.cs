using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Commons.Util;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<VoidValue, VoidVerification>
{
    public GoToTax(ITaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override UseCaseResult<VoidValue, TaxErrorResponse, VoidVerification, TaxErrorVerification> Execute()
    {
        var result = _driver.GoToTax();
        
        return new UseCaseResult<VoidValue, TaxErrorResponse, VoidVerification, TaxErrorVerification>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification(response, ctx),
            (error, ctx) => new TaxErrorVerification(error, ctx));
    }
}
