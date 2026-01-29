using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class BrowseCoupons : BaseShopCommand<BrowseCouponsResponse, BrowseCouponsVerification>
{
    public BrowseCoupons(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override async Task<UseCaseResult<BrowseCouponsResponse, SystemError, BrowseCouponsVerification, ErrorFailureVerification>> Execute()
    {
        var request = new BrowseCouponsRequest();

        var result = await _driver.Coupons().BrowseCoupons(request);

        return new ShopUseCaseResult<BrowseCouponsResponse, BrowseCouponsVerification>(
            result, 
            _context, 
            (response, ctx) => new BrowseCouponsVerification(response, ctx));
    }
}