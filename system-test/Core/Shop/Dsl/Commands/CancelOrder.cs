using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class CancelOrder : BaseShopCommand<VoidValue, VoidVerification<UseCaseContext>>
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

    public override ShopUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var orderNumber = _context.GetResultValue(_orderNumberResultAlias!);
        var result = _driver.CancelOrder(orderNumber);
            
        return new ShopUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
