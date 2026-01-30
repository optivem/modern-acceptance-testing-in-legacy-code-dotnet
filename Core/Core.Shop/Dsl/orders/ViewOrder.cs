using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Common.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class ViewOrder : BaseShopCommand<ViewOrderResponse, ViewOrderVerification>
{
    private string? _orderNumberResultAlias;

    public ViewOrder(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public ViewOrder OrderNumber(string? orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public override async Task<ShopUseCaseResult<ViewOrderResponse, ViewOrderVerification>> Execute()
    {
        var orderNumber = _context.GetResultValue(_orderNumberResultAlias);

        var result = await _driver.Orders().ViewOrder(orderNumber);
        
        return new ShopUseCaseResult<ViewOrderResponse, ViewOrderVerification>(
            result, 
            _context, 
            (response, ctx) => new ViewOrderVerification(response, ctx));
    }
}
