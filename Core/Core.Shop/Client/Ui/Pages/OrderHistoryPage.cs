using Commons.Http;
using Commons.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class OrderHistoryPage : BasePage
{
    private const string OrderNumberInputSelector = "[aria-label='Order Number']";
    private const string SearchButtonSelector = "[aria-label='Refresh Order List']";
    private const string ViewDetailsLinkText = "View Details";
    private const string RowSelectorTemplate = "//tr[contains(., '{0}')]";

    public OrderHistoryPage(PageClient pageClient) : base(pageClient)
    {
    }

    public async Task InputOrderNumberAsync(string? orderNumber)
    {
        await PageClient.FillAsync(OrderNumberInputSelector, orderNumber);
    }

    public async Task ClickSearchAsync()
    {
        await PageClient.ClickAsync(SearchButtonSelector);
    }

    public async Task<bool> IsOrderListedAsync(string? orderNumber)
    {
        var rowSelector = GetRowSelector(orderNumber);
        return await PageClient.IsVisibleAsync(rowSelector);
    }

    public async Task<OrderDetailsPage> ClickViewOrderDetailsAsync(string? orderNumber)
    {
        var rowSelector = GetRowSelector(orderNumber);
        // Find the link by its text content
        var viewDetailsLinkSelector = rowSelector + "//a[contains(text(), '" + ViewDetailsLinkText + "')]";
        await PageClient.ClickAsync(viewDetailsLinkSelector);
        return new OrderDetailsPage(PageClient);
    }

    private string GetRowSelector(string? orderNumber)
    {
        // Simpler selector: find any row that contains the order number text
        return string.Format(RowSelectorTemplate, orderNumber);
    }
}
