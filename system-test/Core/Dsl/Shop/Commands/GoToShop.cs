using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

public class GoToShop : BaseShopCommand<object, VoidVerification>
{
    public GoToShop(IShopDriver driver, TestContext context) 
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
