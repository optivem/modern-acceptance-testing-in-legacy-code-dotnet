using System.Text.RegularExpressions;
using Commons.Http;
using Commons.Playwright;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class NewOrderPage : BasePage
{
    private const string SkuInputSelector = "[aria-label=\"SKU\"]";
    private const string QuantityInputSelector = "[aria-label=\"Quantity\"]";
    private const string CountryInputSelector = "[aria-label=\"Country\"]";
    private const string CouponCodeInputSelector = "[aria-label=\"Coupon Code\"]";
    private const string PlaceOrderButtonSelector = "[aria-label=\"Place Order\"]";
    private const string OrderNumberRegex = @"Success! Order has been created with Order Number ([\w-]+)";
    private const int OrderNumberMatcherGroup = 1;
    private const string OrderNumberNotFoundError = "Could not find order number";

    public NewOrderPage(PageClient pageClient) : base(pageClient)
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

    public void InputCouponCode(string? couponCode)
    {
        PageClient.Fill(CouponCodeInputSelector, couponCode);
    }

    public void ClickPlaceOrder()
    {
        PageClient.Click(PlaceOrderButtonSelector);
    }

    public static string GetOrderNumber(string successMessageText)
    {
        var pattern = new Regex(OrderNumberRegex);
        var match = pattern.Match(successMessageText);

        if (!match.Success)
        {
            throw new InvalidOperationException(OrderNumberNotFoundError);
        }

        return match.Groups[OrderNumberMatcherGroup].Value;
    }
}
