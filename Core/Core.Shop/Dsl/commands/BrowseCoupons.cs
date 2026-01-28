using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class BrowseCoupons : BaseShopCommand<BrowseCouponsResponse, BrowseCouponsVerification>
{
    public BrowseCoupons(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ShopUseCaseResult<BrowseCouponsResponse, BrowseCouponsVerification> Execute()
    {
        var request = new BrowseCouponsRequest();

        var result = _driver.Coupons().BrowseCoupons(request);

        return new ShopUseCaseResult<BrowseCouponsResponse, BrowseCouponsVerification>(
            result, 
            _context, 
            (response, ctx) => new BrowseCouponsVerification(response, ctx));
    }
}