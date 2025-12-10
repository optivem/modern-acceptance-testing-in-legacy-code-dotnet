using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

public class CancelOrder : BaseShopCommand<object, VoidVerification>
{
    private string? _orderNumberResultAlias;

    public CancelOrder(IShopDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public CancelOrder OrderNumber(string orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var orderNumber = Context.GetResultValue(_orderNumberResultAlias!);
        var result = Driver.CancelOrder(orderNumber);
        
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
            
        return new CommandResult<object, VoidVerification>(
            objectResult, 
            Context, 
            (_, ctx) => new VoidVerification(null, ctx));
    }
}
