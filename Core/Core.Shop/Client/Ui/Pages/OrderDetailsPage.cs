using Commons.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;

public class OrderDetailsPage : BasePage
{
    // React uses [role='alert'] directly, without #notifications wrapper - matches Java selectors
    private const string NotificationSelector = "[role='alert']";
    private const string OrderNumberOutputSelector = "[aria-label='Display Order Number']";
    private const string SkuOutputSelector = "[aria-label='Display SKU']";
    private const string CountryOutputSelector = "[aria-label='Display Country']";
    private const string QuantityOutputSelector = "[aria-label='Display Quantity']";
    private const string UnitPriceOutputSelector = "[aria-label='Display Unit Price']";
    private const string BasePriceOutputSelector = "[aria-label='Display Base Price']";
    private const string SubtotalPriceOutputSelector = "[aria-label='Display Subtotal Price']";
    private const string DiscountRateOutputSelector = "[aria-label='Display Discount Rate']";
    private const string DiscountAmountOutputSelector = "[aria-label='Display Discount Amount']";
    private const string TaxRateOutputSelector = "[aria-label='Display Tax Rate']";
    private const string TaxAmountOutputSelector = "[aria-label='Display Tax Amount']";
    private const string TotalPriceOutputSelector = "[aria-label='Display Total Price']";
    private const string StatusOutputSelector = "[aria-label='Display Status']";
    private const string AppliedCouponOutputSelector = "[aria-label='Display Applied Coupon']";
    private const string CancelOrderOutputSelector = "[aria-label='Cancel Order']";

    // Display text constants
    private const string TextNone = "None";
    private const string DollarSymbol = "$";
    private const string PercentSymbol = "%";

    public OrderDetailsPage(PageClient pageClient) : base(pageClient)
    {
    }

    public bool IsLoadedSuccessfully()
    {
        return PageClient.IsVisible(OrderNumberOutputSelector);
    }

    public string GetOrderNumber()
    {
        return PageClient.ReadTextContent(OrderNumberOutputSelector);
    }

    public string GetSku()
    {
        return PageClient.ReadTextContent(SkuOutputSelector);
    }

    public string GetCountry()
    {
        return PageClient.ReadTextContent(CountryOutputSelector);
    }

    public int GetQuantity()
    {
        var textContent = PageClient.ReadTextContent(QuantityOutputSelector);
        return int.Parse(textContent);
    }

    public decimal GetUnitPrice()
    {
        return ReadTextMoney(UnitPriceOutputSelector);
    }

    public decimal GetBasePrice()
    {
        return ReadTextMoney(BasePriceOutputSelector);
    }

    public decimal GetDiscountRate()
    {
        return ReadTextPercentage(DiscountRateOutputSelector);
    }

    public decimal GetDiscountAmount()
    {
        return ReadTextMoney(DiscountAmountOutputSelector);
    }

    public decimal GetSubtotalPrice()
    {
        return ReadTextMoney(SubtotalPriceOutputSelector);
    }

    public decimal GetTaxRate()
    {
        return ReadTextPercentage(TaxRateOutputSelector);
    }

    public decimal GetTaxAmount()
    {
        return ReadTextMoney(TaxAmountOutputSelector);
    }

    public decimal GetTotalPrice()
    {
        return ReadTextMoney(TotalPriceOutputSelector);
    }

    public OrderStatus GetStatus()
    {
        var status = PageClient.ReadTextContent(StatusOutputSelector);
        return Enum.Parse<OrderStatus>(status, true); // true = ignore case
    }

    public string? GetAppliedCoupon()
    {
        var coupon = PageClient.ReadTextContent(AppliedCouponOutputSelector);
        return TextNone.Equals(coupon) ? null : coupon;
    }

    public void ClickCancelOrder()
    {
        PageClient.Click(CancelOrderOutputSelector);
    }

    public bool IsCancelButtonHidden()
    {
        return PageClient.IsHidden(CancelOrderOutputSelector);
    }

    private decimal ReadTextMoney(string selector)
    {
        var textContent = PageClient.ReadTextContent(selector);
        var cleaned = textContent.Replace(DollarSymbol, "").Trim();
        return decimal.Parse(cleaned);
    }

    private decimal ReadTextPercentage(string selector)
    {
        var textContent = PageClient.ReadTextContent(selector);
        var cleaned = textContent.Replace(PercentSymbol, "").Trim();
        var value = decimal.Parse(cleaned);
        return value / 100; // Convert percentage to decimal (e.g., 15% -> 0.15)
    }
}