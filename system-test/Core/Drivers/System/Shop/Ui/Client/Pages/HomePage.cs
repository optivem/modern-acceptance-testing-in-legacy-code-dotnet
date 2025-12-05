using Optivem.Http;
using Optivem.Playwright;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui.Client.Pages;

public class HomePage : BasePage
{
    private const string ShopButtonSelector = "a[href='/shop.html']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history.html']";

    public HomePage(PageGateway pageClient) : base(pageClient)
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
