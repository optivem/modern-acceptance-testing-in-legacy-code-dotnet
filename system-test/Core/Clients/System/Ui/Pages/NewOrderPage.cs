using System.Globalization;
using System.Text.RegularExpressions;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class NewOrderPage
{
    private const string ProductIdInputSelector = "[aria-label=\"Product ID\"]";
    private const string QuantityInputSelector = "[aria-label=\"Quantity\"]";
    private const string CountryInputSelector = "[aria-label=\"Country\"]";
    private const string PlaceOrderButtonSelector = "[aria-label=\"Place Order\"]";
    private const string ConfirmationMessageSelector = "[role='alert']";
    private const string OrderNumberRegex = @"Order Number: (ORD-[\w-]+)";
    private const int OrderNumberMatcherGroup = 1;
    private const string OriginalPriceRegex = @"Original Price \$(\d+(?:\.\d{2})?)";
    private const int OriginalPriceMatcherGroup = 1;

    private readonly TestPageClient _pageClient;

    public NewOrderPage(TestPageClient pageClient)
    {
        _pageClient = pageClient;
    }

    public async Task InputProductIdAsync(string productId)
    {
        await _pageClient.FillAsync(ProductIdInputSelector, productId);
    }

    public async Task InputQuantityAsync(string quantity)
    {
        await _pageClient.FillAsync(QuantityInputSelector, quantity);
    }

    public async Task InputCountryAsync(string country)
    {
        await _pageClient.FillAsync(CountryInputSelector, country);
    }

    public async Task ClickPlaceOrderAsync()
    {
        await _pageClient.ClickAsync(PlaceOrderButtonSelector);
    }

    public async Task<string?> ReadConfirmationMessageTextAsync()
    {
        return await _pageClient.ReadTextContentAsync(ConfirmationMessageSelector);
    }

    public async Task<string> ExtractOrderNumberAsync()
    {
        var confirmationMessageText = await ReadConfirmationMessageTextAsync();
        var pattern = new Regex(OrderNumberRegex);
        var matcher = pattern.Match(confirmationMessageText ?? "");
        
        Assert.True(matcher.Success, 
            $"Should extract order number from confirmation message: {confirmationMessageText}");
        
        return matcher.Groups[OrderNumberMatcherGroup].Value;
    }

    public async Task<decimal> ExtractOriginalPriceAsync()
    {
        var confirmationMessageText = await ReadConfirmationMessageTextAsync();
        var pattern = new Regex(OriginalPriceRegex);
        var matcher = pattern.Match(confirmationMessageText ?? "");
        
        Assert.True(matcher.Success, 
            $"Should extract original price from confirmation message: {confirmationMessageText}");
        
        var priceString = matcher.Groups[OriginalPriceMatcherGroup].Value;
        return decimal.Parse(priceString, CultureInfo.InvariantCulture);
    }

    public async Task AssertOrderPlacedSuccessfullyAsync()
    {
        var confirmationMessageText = await ReadConfirmationMessageTextAsync();
        Assert.NotNull(confirmationMessageText);
        Assert.Contains("Order placed successfully", confirmationMessageText);
        Assert.Contains("Order Number:", confirmationMessageText);
    }

    public async Task AssertValidationErrorAsync(string expectedError)
    {
        var confirmationMessageText = await ReadConfirmationMessageTextAsync();
        Assert.NotNull(confirmationMessageText);
        Assert.Contains(expectedError, confirmationMessageText);
    }

    public async Task AssertNewOrderPageLoadedAsync()
    {
        var heading = await _pageClient.ReadTextContentAsync("h1");
        Assert.Equal("Shop", heading);
    }
}
