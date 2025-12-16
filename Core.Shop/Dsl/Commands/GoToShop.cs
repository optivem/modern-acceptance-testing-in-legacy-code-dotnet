using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class GoToShop : BaseShopCommand<VoidValue, VoidResponseVerification<UseCaseContext>>
{
    public GoToShop(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ShopUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToShop();
        return new ShopUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidResponseVerification<UseCaseContext>(response, ctx));
    }
}
