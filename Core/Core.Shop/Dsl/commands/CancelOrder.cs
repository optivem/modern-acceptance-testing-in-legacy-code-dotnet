using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class CancelOrder : BaseShopCommand<VoidValue, VoidVerification>
{
    private string? _orderNumberResultAlias;

    public CancelOrder(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public CancelOrder OrderNumber(string orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public override ShopUseCaseResult<VoidValue, VoidVerification> Execute()
    {
        var orderNumber = _context.GetResultValue(_orderNumberResultAlias!);
        var result = _driver.Orders().CancelOrder(orderNumber);
            
        return new ShopUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
