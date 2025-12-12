using System.Text.RegularExpressions;
using Optivem.Http;
using Optivem.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;

public class NewOrderPage : BasePage
{
    private const string SkuInputSelector = "[aria-label=\"SKU\"]";
    private const string QuantityInputSelector = "[aria-label=\"Quantity\"]";
    private const string CountryInputSelector = "[aria-label=\"Country\"]";
    private const string PlaceOrderButtonSelector = "[aria-label=\"Place Order\"]";
    private const string OrderNumberRegex = @"Success! Order has been created with Order Number ([\w-]+)";
    private const int OrderNumberMatcherGroup = 1;

    public NewOrderPage(PageGateway pageClient) : base(pageClient)
    {
    }

    public void InputSku(string? sku)
    {
        PageClient.Fill(SkuInputSelector, sku);
    }

    public void InputQuantity(string? quantity)
    {
        PageClient.Fill(QuantityInputSelector, quantity);
    }

    public void InputCountry(string? country)
    {
        PageClient.Fill(CountryInputSelector, country);
    }

    public void ClickPlaceOrder()
    {
        PageClient.Click(PlaceOrderButtonSelector);
    }

    public string GetOrderNumber()
    {
        var confirmationMessageText = ReadSuccessNotification();

        var pattern = new Regex(OrderNumberRegex);
        var match = pattern.Match(confirmationMessageText);

        if (!match.Success)
        {
            throw new InvalidOperationException("Could not find order number");
        }

        return match.Groups[OrderNumberMatcherGroup].Value;
    }
}
