using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class ViewOrder : BaseShopCommand<GetOrderResponse, ViewOrderVerification>
{
    private string? _orderNumberResultAlias;

    public ViewOrder(IShopDriver driver, Context context) 
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
