using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;

public class OrderHistoryPage : BasePage
{
    private const string OrderNumberInputSelector = "[aria-label='Order Number']";
    private const string SearchButtonSelector = "[aria-label='Search']";
    private const string OrderDetailsContainerSelector = "#orderDetails";
    private const string OrderNumberOutputSelector = "[aria-label='Display Order Number']";
    private const string SkuOutputSelector = "[aria-label='Display SKU']";
    private const string CountryOutputSelector = "[aria-label='Display Country']";
    private const string QuantityOutputSelector = "[aria-label='Display Quantity']";
    private const string UnitPriceOutputSelector = "[aria-label='Display Unit Price']";
    private const string SubtotalPriceOutputSelector = "[aria-label='Display Subtotal Price']";
    private const string DiscountRateOutputSelector = "[aria-label='Display Discount Rate']";
    private const string DiscountAmountOutputSelector = "[aria-label='Display Discount Amount']";
    private const string PreTaxTotalOutputSelector = "[aria-label='Display Pre-Tax Total']";
    private const string TaxRateOutputSelector = "[aria-label='Display Tax Rate']";
    private const string TaxAmountOutputSelector = "[aria-label='Display Tax Amount']";
    private const string TotalPriceOutputSelector = "[aria-label='Display Total Price']";
    private const string StatusOutputSelector = "[aria-label='Display Status']";
    private const string CancelOrderOutputSelector = "[aria-label='Cancel Order']";
    private const string OrderDetailsHeadingText = "Order Details";

    public OrderHistoryPage(PageClient pageClient) : base(pageClient)
    {
    }

    public void InputOrderNumber(string orderNumber)
    {
        PageClient.Fill(OrderNumberInputSelector, orderNumber);
    }

    public void ClickSearch()
    {
        PageClient.Click(SearchButtonSelector);
    }

    public void WaitForOrderDetails()
    {
        var orderDetailsText = PageClient.ReadTextContent(OrderDetailsContainerSelector);
        if (!orderDetailsText.Contains(OrderDetailsHeadingText))
        {
            throw new InvalidOperationException("Should display order details heading");
        }
    }

    public bool HasOrderDetails()
    {
        try
        {
            WaitForOrderDetails();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetOrderNumber() => PageClient.ReadInputValue(OrderNumberOutputSelector);
    public string GetSku() => PageClient.ReadInputValue(SkuOutputSelector);
    public string GetCountry() => PageClient.ReadInputValue(CountryOutputSelector);
    public int GetQuantity() => PageClient.ReadInputIntegerValue(QuantityOutputSelector);
    public decimal GetUnitPrice() => PageClient.ReadInputCurrencyDecimalValue(UnitPriceOutputSelector);
    public decimal GetSubtotalPrice() => PageClient.ReadInputCurrencyDecimalValue(SubtotalPriceOutputSelector);
    public decimal GetDiscountRate() => PageClient.ReadInputPercentageDecimalValue(DiscountRateOutputSelector);
    public decimal GetDiscountAmount() => PageClient.ReadInputCurrencyDecimalValue(DiscountAmountOutputSelector);
    public decimal GetPreTaxTotal() => PageClient.ReadInputCurrencyDecimalValue(PreTaxTotalOutputSelector);
    public decimal GetTaxRate() => PageClient.ReadInputPercentageDecimalValue(TaxRateOutputSelector);
    public decimal GetTaxAmount() => PageClient.ReadInputCurrencyDecimalValue(TaxAmountOutputSelector);
    public decimal GetTotalPrice() => PageClient.ReadInputCurrencyDecimalValue(TotalPriceOutputSelector);

    public OrderStatus GetStatus()
    {
        var status = PageClient.ReadInputValue(StatusOutputSelector);
        return Enum.Parse<OrderStatus>(status);
    }

    public void ClickCancelOrder()
    {
        PageClient.Click(CancelOrderOutputSelector);
        PageClient.WaitForHidden(CancelOrderOutputSelector);
    }

    public bool IsCancelButtonHidden()
    {
        return PageClient.IsHidden(CancelOrderOutputSelector);
    }
}
