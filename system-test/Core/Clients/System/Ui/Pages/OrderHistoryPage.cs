using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos.Enums;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

public class OrderHistoryPage : BasePage
{
    private const string OrderNumberInputSelector = "#orderNumber";
    private const string SearchButtonSelector = "[aria-label=\"Search\"]";
    private const string OrderDetailsContainerSelector = "#orderDetails";
    private const string OrderDetailsHeadingText = "Order Details";
    private const string OrderNumberOutputSelector = "#displayOrderNumber";
    private const string SkuOutputSelector = "#displaySku";
    private const string QuantityOutputSelector = "#displayQuantity";
    private const string CountryOutputSelector = "#displayCountry";
    private const string UnitPriceOutputSelector = "#displayUnitPrice";
    private const string OriginalPriceOutputSelector = "#displayOriginalPrice";
    private const string DiscountRateOutputSelector = "#displayDiscountRate";
    private const string DiscountAmountOutputSelector = "#displayDiscountAmount";
    private const string SubtotalPriceOutputSelector = "#displaySubtotalPrice";
    private const string TaxRateOutputSelector = "#displayTaxRate";
    private const string TaxAmountOutputSelector = "#displayTaxAmount";
    private const string TotalPriceOutputSelector = "#displayTotalPrice";
    private const string StatusOutputSelector = "#displayStatus";
    private const string CancelOrderButtonSelector = "[aria-label=\"Cancel Order\"]";

    public OrderHistoryPage(TestPageClient pageClient) : base(pageClient)
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
            throw new Exception("Should display order details heading");
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

    public string GetOrderNumber()
    {
        return PageClient.ReadInputValue(OrderNumberOutputSelector);
    }

    public string GetSku()
    {
        return PageClient.ReadInputValue(SkuOutputSelector);
    }

    public string GetCountry()
    {
        return PageClient.ReadInputValue(CountryOutputSelector);
    }

    public string GetQuantity()
    {
        return PageClient.ReadInputValue(QuantityOutputSelector);
    }

    public decimal GetUnitPrice()
    {
        return PageClient.ReadInputCurrencyDecimalValue(UnitPriceOutputSelector);
    }

    public decimal GetOriginalPrice()
    {
        return PageClient.ReadInputCurrencyDecimalValue(OriginalPriceOutputSelector);
    }

    public decimal GetDiscountRate()
    {
        return PageClient.ReadInputPercentageDecimalValue(DiscountRateOutputSelector);
    }

    public decimal GetDiscountAmount()
    {
        return PageClient.ReadInputCurrencyDecimalValue(DiscountAmountOutputSelector);
    }

    public decimal GetSubtotalPrice()
    {
        return PageClient.ReadInputCurrencyDecimalValue(SubtotalPriceOutputSelector);
    }

    public decimal GetTaxRate()
    {
        return PageClient.ReadInputPercentageDecimalValue(TaxRateOutputSelector);
    }

    public decimal GetTaxAmount()
    {
        return PageClient.ReadInputCurrencyDecimalValue(TaxAmountOutputSelector);
    }

    public decimal GetTotalPrice()
    {
        return PageClient.ReadInputCurrencyDecimalValue(TotalPriceOutputSelector);
    }

    public OrderStatus GetStatus()
    {
        var statusText = PageClient.ReadInputValue(StatusOutputSelector);
        return Enum.Parse<OrderStatus>(statusText, ignoreCase: true);
    }

    public void ClickCancelOrder()
    {
        PageClient.Click(CancelOrderButtonSelector);
    }
}
