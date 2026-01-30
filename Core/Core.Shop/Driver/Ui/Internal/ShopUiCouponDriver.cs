using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Optivem.EShop.SystemTest.Core.Shop.Commons;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using static Optivem.EShop.SystemTest.Core.Shop.Commons.SystemResults;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Internal;

public class ShopUiCouponDriver : ICouponDriver
{
    private readonly Func<Task<HomePage>> _homePageSupplier;
    private readonly PageNavigator _pageNavigator;
    private CouponManagementPage? _couponManagementPage;

    public ShopUiCouponDriver(Func<Task<HomePage>> homePageSupplier, PageNavigator pageNavigator)
    {
        _homePageSupplier = homePageSupplier;
        _pageNavigator = pageNavigator;
    }

    public async Task<Result<VoidValue, SystemError>> PublishCoupon(PublishCouponRequest request)
    {
        await EnsureOnCouponManagementPageAsync();
        
        await _couponManagementPage!.InputCouponCodeAsync(request.Code);
        await _couponManagementPage.InputDiscountRateAsync(request.DiscountRate);
        await _couponManagementPage.InputValidFromAsync(request.ValidFrom);
        await _couponManagementPage.InputValidToAsync(request.ValidTo);
        await _couponManagementPage.InputUsageLimitAsync(request.UsageLimit);
        await _couponManagementPage.ClickPublishCouponAsync();
        
        var result = await _couponManagementPage.GetResultAsync();
        return result.MapVoid();
    }

    public async Task<Result<BrowseCouponsResponse, SystemError>> BrowseCoupons(BrowseCouponsRequest request)
    {
        // Always navigate fresh to ensure we get the latest coupon data (e.g., updated used counts)
        await NavigateToCouponManagementPageAsync();
        
        var coupons = await _couponManagementPage!.ReadCouponsAsync();
        
        var response = new BrowseCouponsResponse
        {
            Coupons = coupons
        };
        
        return Success(response);
    }

    private async Task EnsureOnCouponManagementPageAsync()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.COUPON_MANAGEMENT))
        {
            await NavigateToCouponManagementPageAsync();
        }
    }

    private async Task NavigateToCouponManagementPageAsync()
    {
        var homePage = await _homePageSupplier();
        _couponManagementPage = await homePage.ClickCouponManagementAsync();
        _pageNavigator.SetCurrentPage(PageNavigator.Page.COUPON_MANAGEMENT);
    }
}