using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Verifications;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

public class ViewOrder : BaseShopCommand<GetOrderResponse, ViewOrderVerification>
{
    private string? _orderNumberResultAlias;

    public ViewOrder(IShopDriver driver, TestContext context) 
        : base(driver, context)
    {
    }

    public ViewOrder OrderNumber(string orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public override CommandResult<GetOrderResponse, ViewOrderVerification> Execute()
    {
        var orderNumber = Context.GetResultValue(_orderNumberResultAlias!);
        var result = Driver.ViewOrder(orderNumber);
        
        return new CommandResult<GetOrderResponse, ViewOrderVerification>(
            result, 
            Context, 
            (response, ctx) => new ViewOrderVerification(response, ctx));
    }
}
