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

    public NewOrderPage ClickNewOrder()
    {
        PageClient.Click(ShopButtonSelector);
        return new NewOrderPage(PageClient);
    }

    public OrderHistoryPage ClickOrderHistory()
    {
        PageClient.Click(OrderHistoryButtonSelector);
        return new OrderHistoryPage(PageClient);
    }

    public CouponManagementPage ClickCouponManagement()
    {
        PageClient.Click(CouponManagementButtonSelector);
        return new CouponManagementPage(PageClient);
    }
}
