using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Commons.Util;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class GoToShop : BaseShopCommand<VoidValue, VoidVerification>
{
    public GoToShop(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override async Task<UseCaseResult<VoidValue, SystemError, VoidVerification, ErrorFailureVerification>> Execute()
    {
        var result = await _driver.GoToShop();
        
        return new ShopUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
