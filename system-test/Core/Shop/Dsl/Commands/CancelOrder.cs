using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class CancelOrder : BaseShopCommand<VoidValue, VoidVerification>
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

    public override CommandResult<VoidValue, VoidVerification> Execute()
    {
        var orderNumber = _context.GetResultValue(_orderNumberResultAlias!);
        var result = _driver.CancelOrder(orderNumber);
            
        return new CommandResult<VoidValue, VoidVerification>(
            result, 
            _context, 
            (_, ctx) => new VoidVerification(ctx));
    }
}
