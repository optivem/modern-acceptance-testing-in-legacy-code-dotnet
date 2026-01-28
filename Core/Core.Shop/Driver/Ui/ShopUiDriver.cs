using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Internal;
using static Optivem.EShop.SystemTest.Core.Shop.Commons.SystemResults;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;

public class ShopUiDriver : IShopDriver
{
    private readonly ShopUiClient _client;
    private readonly PageNavigator _pageNavigator;
    private readonly IOrderDriver _orderDriver;
    private readonly ICouponDriver _couponDriver;
    
    private HomePage? _homePage;

    public ShopUiDriver(string baseUrl)
    {
        _client = new ShopUiClient(baseUrl);
        
        _pageNavigator = new PageNavigator();
        _orderDriver = new ShopUiOrderDriver(() => GetHomePage(), _pageNavigator);
        _couponDriver = new ShopUiCouponDriver(() => GetHomePage(), _pageNavigator);
    }

    public Result<VoidValue, SystemError> GoToShop()
    {
        _homePage = _client.OpenHomePage();
        
        if (!_client.IsStatusOk() || !_client.IsPageLoaded())
        {
            return Failure<VoidValue>("Failed to load home page");
        }
        
        _pageNavigator.SetCurrentPage(PageNavigator.Page.HOME);
        
        return Success();
    }

    public IOrderDriver Orders() => _orderDriver;
    
    public ICouponDriver Coupons() => _couponDriver;

    private HomePage GetHomePage()
    {
        if (_homePage == null || !_pageNavigator.IsOnPage(PageNavigator.Page.HOME))
        {
            _homePage = _client.OpenHomePage();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.HOME);
        }
        return _homePage;
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}