using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class OrderHistoryPage
{
    private const string OrderNumberInputSelector = "[aria-label=\"Order Number\"]";
    private const string ViewOrderButtonSelector = "[aria-label=\"View Order\"]";
    private const string CancelOrderButtonSelector = "[aria-label=\"Cancel Order\"]";
    private const string OrderNumberOutputSelector = "[aria-label=\"Order Number Output\"]";
    private const string SkuOutputSelector = "[aria-label=\"SKU Output\"]";
    private const string QuantityOutputSelector = "[aria-label=\"Quantity Output\"]";
    private const string UnitPriceOutputSelector = "[aria-label=\"Unit Price Output\"]";
    private const string OriginalPriceOutputSelector = "[aria-label=\"Original Price Output\"]";
    private const string DiscountRateOutputSelector = "[aria-label=\"Discount Rate Output\"]";
    private const string DiscountAmountOutputSelector = "[aria-label=\"Discount Amount Output\"]";
    private const string SubtotalPriceOutputSelector = "[aria-label=\"Subtotal Price Output\"]";
    private const string TaxRateOutputSelector = "[aria-label=\"Tax Rate Output\"]";
    private const string TaxAmountOutputSelector = "[aria-label=\"Tax Amount Output\"]";
    private const string TotalPriceOutputSelector = "[aria-label=\"Total Price Output\"]";
    private const string StatusOutputSelector = "[aria-label=\"Status Output\"]";
    private const string CountryOutputSelector = "[aria-label=\"Country Output\"]";
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
}
