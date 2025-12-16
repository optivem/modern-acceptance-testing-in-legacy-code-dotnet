using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class ViewOrder : BaseShopCommand<GetOrderResponse, ViewOrderVerification>
{
    private string? _orderNumberResultAlias;

    public ViewOrder(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public ViewOrder OrderNumber(string orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public override ShopUseCaseResult<GetOrderResponse, ViewOrderVerification> Execute()
    {
        var orderNumber = _context.GetResultValue(_orderNumberResultAlias!);
        var result = _driver.ViewOrder(orderNumber);
        
        return new ShopUseCaseResult<GetOrderResponse, ViewOrderVerification>(
            result, 
            _context, 
            (response, ctx) => new ViewOrderVerification(response, ctx));
    }
}
