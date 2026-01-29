using Commons.Http;
using Commons.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class HomePage : BasePage
{
    private const string ShopButtonSelector = "a[href='/shop']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history']";
    private const string CouponManagementButtonSelector = "a[href='/admin-coupons']";

    public HomePage(PageClient pageClient) : base(pageClient)
    {
    }

    public async Task<NewOrderPage> ClickNewOrderAsync()
    {
        await PageClient.ClickAsync(ShopButtonSelector);
        return new NewOrderPage(PageClient);
    }

    public async Task<OrderHistoryPage> ClickOrderHistoryAsync()
    {
        await PageClient.ClickAsync(OrderHistoryButtonSelector);
        return new OrderHistoryPage(PageClient);
    }

    public async Task<CouponManagementPage> ClickCouponManagementAsync()
    {
        await PageClient.ClickAsync(CouponManagementButtonSelector);
        return new CouponManagementPage(PageClient);
    }
}
