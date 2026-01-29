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
    private readonly Func<HomePage> _homePageSupplier;
    private readonly PageNavigator _pageNavigator;
    private CouponManagementPage? _couponManagementPage;

    public ShopUiCouponDriver(Func<HomePage> homePageSupplier, PageNavigator pageNavigator)
    {
        _homePageSupplier = homePageSupplier;
        _pageNavigator = pageNavigator;
    }

    public Task<Result<VoidValue, SystemError>> PublishCoupon(PublishCouponRequest request)
    {
        EnsureOnCouponManagementPage();
        
        _couponManagementPage!.InputCouponCode(request.Code);
        _couponManagementPage.InputDiscountRate(request.DiscountRate);
        _couponManagementPage.InputValidFrom(request.ValidFrom);
        _couponManagementPage.InputValidTo(request.ValidTo);
        _couponManagementPage.InputUsageLimit(request.UsageLimit);
        _couponManagementPage.ClickPublishCoupon();
        
        return Task.FromResult(_couponManagementPage.GetResult().MapVoid());
    }

    public Task<Result<BrowseCouponsResponse, SystemError>> BrowseCoupons(BrowseCouponsRequest request)
    {
        // Always navigate fresh to ensure we get the latest coupon data (e.g., updated used counts)
        NavigateToCouponManagementPage();
        
        var coupons = _couponManagementPage!.ReadCoupons();
        
        var response = new BrowseCouponsResponse
        {
            Coupons = coupons
        };
        
        return Task.FromResult(Success(response));
    }

    private void EnsureOnCouponManagementPage()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.COUPON_MANAGEMENT))
        {
            NavigateToCouponManagementPage();
        }
    }

    private void NavigateToCouponManagementPage()
    {
        var homePage = _homePageSupplier();
        _couponManagementPage = homePage.ClickCouponManagement();
        _pageNavigator.SetCurrentPage(PageNavigator.Page.COUPON_MANAGEMENT);
    }
}