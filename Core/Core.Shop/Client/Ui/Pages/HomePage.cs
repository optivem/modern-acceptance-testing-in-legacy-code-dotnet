using Optivem.Commons.Http;
using Optivem.Commons.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class HomePage : BasePage
{
    private const string ShopButtonSelector = "a[href='/shop']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history']";

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
