using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class HomePage
{
    private const string ShopButtonSelector = "a[href='/shop.html']";
    private const string OrderHistoryButtonSelector = "a[href='/order-history.html']";

    private readonly TestPageClient _pageClient;

    public HomePage(TestPageClient pageClient)
    {
        _pageClient = pageClient;
    }

    public async Task<NewOrderPage> ClickNewOrderAsync()
    {
        await _pageClient.ClickAsync(ShopButtonSelector);
        return new NewOrderPage(_pageClient);
    }

    public async Task<OrderHistoryPage> ClickOrderHistoryAsync()
    {
        await _pageClient.ClickAsync(OrderHistoryButtonSelector);
        return new OrderHistoryPage(_pageClient);
    }
}
