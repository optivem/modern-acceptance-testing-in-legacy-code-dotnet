using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands.Base;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

public class GoToErp : BaseErpCommand<object, VoidVerification>
{
    public GoToErp(ErpApiDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var result = Driver.GoToErp();
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
        return new CommandResult<object, VoidVerification>(objectResult, Context, (_, ctx) => new VoidVerification(null, ctx));
    }
}
