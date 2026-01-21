using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class GoToShop : BaseShopCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    public GoToShop(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ShopUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToShop();
        return new ShopUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
