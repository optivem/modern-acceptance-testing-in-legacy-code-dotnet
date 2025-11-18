using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class OrderHistoryPage
{
    private const string OrderNumberInputSelector = "[aria-label=\"Order Number\"]";
    private const string ViewOrderButtonSelector = "[aria-label=\"Search\"]";
    private const string CancelOrderButtonSelector = "[aria-label=\"Cancel Order\"]";
    private const string OrderNumberOutputSelector = "[aria-label=\"Display Order Number\"]";
    private const string SkuOutputSelector = "[aria-label=\"Display Product ID\"]";
    private const string QuantityOutputSelector = "[aria-label=\"Display Quantity\"]";
    private const string UnitPriceOutputSelector = "[aria-label=\"Display Unit Price\"]";
    private const string OriginalPriceOutputSelector = "[aria-label=\"Display Original Price\"]";
    private const string DiscountRateOutputSelector = "[aria-label=\"Display Discount Rate\"]";
    private const string DiscountAmountOutputSelector = "[aria-label=\"Display Discount Amount\"]";
    private const string SubtotalPriceOutputSelector = "[aria-label=\"Display Subtotal Price\"]";
    private const string TaxRateOutputSelector = "[aria-label=\"Display Tax Rate\"]";
    private const string TaxAmountOutputSelector = "[aria-label=\"Display Tax Amount\"]";
    private const string TotalPriceOutputSelector = "[aria-label=\"Display Total Price\"]";
    private const string StatusOutputSelector = "[aria-label=\"Display Status\"]";
    private const string CountryOutputSelector = "[aria-label=\"Display Country\"]";
    private const string ConfirmationMessageSelector = "[role='alert']";

    private readonly TestPageClient _pageClient;

    public OrderHistoryPage(TestPageClient pageClient)
    {
        _pageClient = pageClient;
    }

    public async Task InputOrderNumberAsync(string orderNumber)
    {
        await _pageClient.FillAsync(OrderNumberInputSelector, orderNumber);
    }

    public async Task ClickViewOrderAsync()
    {
        await _pageClient.ClickAsync(ViewOrderButtonSelector);
    }

    public async Task ClickCancelOrderAsync()
    {
        await _pageClient.ClickAsync(CancelOrderButtonSelector);
    }

    public async Task<string> GetOrderNumberAsync()
    {
        return await _pageClient.ReadInputValueAsync(OrderNumberOutputSelector);
    }

    public async Task<string> GetSkuAsync()
    {
        return await _pageClient.ReadInputValueAsync(SkuOutputSelector);
    }

    public async Task<string> GetQuantityAsync()
    {
        return await _pageClient.ReadInputValueAsync(QuantityOutputSelector);
    }

    public async Task<string> GetUnitPriceAsync()
    {
        return await _pageClient.ReadInputValueAsync(UnitPriceOutputSelector);
    }

    public async Task<string> GetOriginalPriceAsync()
    {
        return await _pageClient.ReadInputValueAsync(OriginalPriceOutputSelector);
    }

    public async Task<string> GetDiscountRateAsync()
    {
        return await _pageClient.ReadInputValueAsync(DiscountRateOutputSelector);
    }

    public async Task<string> GetDiscountAmountAsync()
    {
        return await _pageClient.ReadInputValueAsync(DiscountAmountOutputSelector);
    }

    public async Task<string> GetSubtotalPriceAsync()
    {
        return await _pageClient.ReadInputValueAsync(SubtotalPriceOutputSelector);
    }

    public async Task<string> GetTaxRateAsync()
    {
        return await _pageClient.ReadInputValueAsync(TaxRateOutputSelector);
    }

    public async Task<string> GetTaxAmountAsync()
    {
        return await _pageClient.ReadInputValueAsync(TaxAmountOutputSelector);
    }

    public async Task<string> GetTotalPriceAsync()
    {
        return await _pageClient.ReadInputValueAsync(TotalPriceOutputSelector);
    }

    public async Task<string> GetStatusAsync()
    {
        return await _pageClient.ReadInputValueAsync(StatusOutputSelector);
    }

    public async Task<string> GetCountryAsync()
    {
        return await _pageClient.ReadInputValueAsync(CountryOutputSelector);
    }

    public async Task<string?> ReadConfirmationMessageAsync()
    {
        return await _pageClient.ReadTextContentAsync(ConfirmationMessageSelector);
    }

    public async Task<bool> IsCancelButtonHiddenAsync()
    {
        return await _pageClient.IsHiddenAsync(CancelOrderButtonSelector);
    }

    public async Task WaitForCancelButtonHiddenAsync()
    {
        await _pageClient.WaitForHiddenAsync(CancelOrderButtonSelector);
    }

    public async Task<string> GetDisplayedOrderNumberAsync()
    {
        return await GetOrderNumberAsync();
    }

    public async Task<string> GetDisplayedSkuAsync()
    {
        return await GetSkuAsync();
    }

    public async Task<string> GetDisplayedQuantityAsync()
    {
        return await GetQuantityAsync();
    }

    public async Task<string> GetDisplayedCountryAsync()
    {
        return await GetCountryAsync();
    }

    public async Task<string> GetDisplayedUnitPriceAsync()
    {
        return await GetUnitPriceAsync();
    }

    public async Task<string> GetDisplayedOriginalPriceAsync()
    {
        return await GetOriginalPriceAsync();
    }

    public async Task<string> GetDisplayedStatusAsync()
    {
        return await GetStatusAsync();
    }

    public async Task AssertOrderDetailsDisplayedAsync()
    {
        // Wait for order details to be visible
        await Task.Delay(500); // Small delay to allow page to render
    }

    public async Task AssertOrderNotFoundAsync()
    {
        var confirmationMessage = await ReadConfirmationMessageAsync();
        Assert.NotNull(confirmationMessage);
        Assert.Contains("Order not found", confirmationMessage);
    }

    public async Task AssertOrderCancelledAsync()
    {
        await Task.Delay(1000); // Wait for page refresh after cancellation
        var status = await GetStatusAsync();
        Assert.Equal("CANCELLED", status);
    }

    public async Task AssertCancelButtonNotVisibleAsync()
    {
        var isHidden = await IsCancelButtonHiddenAsync();
        Assert.True(isHidden, "Cancel button should not be visible");
    }

    public async Task AssertOrderHistoryPageLoadedAsync()
    {
        var heading = await _pageClient.ReadTextContentAsync("h1");
        Assert.Equal("Order History", heading);
    }
}
