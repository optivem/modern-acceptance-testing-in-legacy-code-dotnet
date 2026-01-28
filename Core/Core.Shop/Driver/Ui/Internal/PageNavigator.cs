namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Internal;

public class PageNavigator
{
    private Page _currentPage = Page.NONE;

    public bool IsOnPage(Page page)
    {
        return _currentPage == page;
    }

    public void SetCurrentPage(Page page)
    {
        _currentPage = page;
    }

    public Page GetCurrentPage()
    {
        return _currentPage;
    }

    public enum Page
    {
        NONE,
        HOME,
        NEW_ORDER,
        ORDER_HISTORY,
        ORDER_DETAILS,
        COUPON_MANAGEMENT
    }
}