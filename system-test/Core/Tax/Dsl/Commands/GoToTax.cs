using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class GoToTax : BaseTaxCommand<object, VoidVerification>
{
    public GoToTax(TaxDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var result = Driver.GoToTax();
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
        return new CommandResult<object, VoidVerification>(objectResult, Context, (_, ctx) => new VoidVerification(null, ctx));
    }
}
