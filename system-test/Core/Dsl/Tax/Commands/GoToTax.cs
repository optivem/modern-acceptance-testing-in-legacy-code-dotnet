using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;
using Optivem.EShop.SystemTest.Core.Dsl.Tax.Commands.Base;

namespace Optivem.EShop.SystemTest.Core.Dsl.Tax.Commands;

public class GoToTax : BaseTaxCommand<object, VoidVerification>
{
    public GoToTax(TaxApiDriver driver, Context context) 
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
