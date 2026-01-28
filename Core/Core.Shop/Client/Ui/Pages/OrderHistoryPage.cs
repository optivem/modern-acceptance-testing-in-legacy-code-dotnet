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

    public void InputOrderNumber(string? orderNumber)
    {
        PageClient.Fill(OrderNumberInputSelector, orderNumber);
    }

    public void ClickSearch()
    {
        PageClient.Click(SearchButtonSelector);
    }

    public bool IsOrderListed(string? orderNumber)
    {
        var rowSelector = GetRowSelector(orderNumber);
        return PageClient.IsVisible(rowSelector);
    }

    public OrderDetailsPage ClickViewOrderDetails(string? orderNumber)
    {
        var rowSelector = GetRowSelector(orderNumber);
        // Find the link by its text content
        var viewDetailsLinkSelector = rowSelector + "//a[contains(text(), '" + ViewDetailsLinkText + "')]";
        PageClient.Click(viewDetailsLinkSelector);
        return new OrderDetailsPage(PageClient);
    }

    private string GetRowSelector(string? orderNumber)
    {
        // Simpler selector: find any row that contains the order number text
        return string.Format(RowSelectorTemplate, orderNumber);
    }
}
