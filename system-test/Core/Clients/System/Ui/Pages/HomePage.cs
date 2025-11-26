using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class HomePage : BasePage
{
    private const string ShopButtonSelector = "a[href='/shop.html']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history.html']";

    public HomePage(TestPageClient pageClient) : base(pageClient)
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
