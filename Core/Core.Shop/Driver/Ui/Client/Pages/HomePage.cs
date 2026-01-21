using Optivem.Commons.Http;
using Optivem.Commons.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;

public class HomePage : BasePage
{
    private const string ShopButtonSelector = "a[href='/shop.html']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history.html']";

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
}
