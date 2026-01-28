using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class GoToShop : BaseShopCommand<VoidValue, VoidVerification>
{
    public GoToShop(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ShopUseCaseResult<VoidValue, VoidVerification> Execute()
    {
        var result = _driver.GoToShop();
        
        return new ShopUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
