using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

public class GoToShop : BaseShopCommand<object, VoidVerification>
{
    public GoToShop(IShopDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var result = Driver.GoToShop();
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
        return new CommandResult<object, VoidVerification>(objectResult, Context, (_, ctx) => new VoidVerification(null, ctx));
    }
}
