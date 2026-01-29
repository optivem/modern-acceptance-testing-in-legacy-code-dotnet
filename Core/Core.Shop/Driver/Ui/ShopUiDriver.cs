using Commons.Util;
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

    private ShopUiDriver(ShopUiClient client, PageNavigator pageNavigator)
    {
        _client = client;
        _pageNavigator = pageNavigator;
        _orderDriver = new ShopUiOrderDriver(() => GetHomePage(), _pageNavigator);
        _couponDriver = new ShopUiCouponDriver(() => GetHomePage(), _pageNavigator);
    }

    public static async Task<ShopUiDriver> CreateAsync(string baseUrl)
    {
        var client = await ShopUiClient.CreateAsync(baseUrl);
        var pageNavigator = new PageNavigator();
        return new ShopUiDriver(client, pageNavigator);
    }

    public async Task<Result<VoidValue, SystemError>> GoToShop()
    {
        _homePage = await _client.OpenHomePageAsync();
        
        if (!_client.IsStatusOk() || !await _client.IsPageLoadedAsync())
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
            _homePage = _client.OpenHomePageAsync().GetAwaiter().GetResult();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.HOME);
        }
        return _homePage;
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}