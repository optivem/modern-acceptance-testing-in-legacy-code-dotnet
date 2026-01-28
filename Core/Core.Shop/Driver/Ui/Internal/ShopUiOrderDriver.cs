using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using static Optivem.EShop.SystemTest.Core.Shop.Commons.SystemResults;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Internal;

public class ShopUiOrderDriver : IOrderDriver
{
    private readonly Func<HomePage> _homePageSupplier;
    private readonly PageNavigator _pageNavigator;
    
    private NewOrderPage? _newOrderPage;
    private OrderHistoryPage? _orderHistoryPage;
    private OrderDetailsPage? _orderDetailsPage;

    public ShopUiOrderDriver(Func<HomePage> homePageSupplier, PageNavigator pageNavigator)
    {
        _homePageSupplier = homePageSupplier;
        _pageNavigator = pageNavigator;
    }

    public Result<PlaceOrderResponse, SystemError> PlaceOrder(PlaceOrderRequest request)
    {
        var sku = request.Sku;
        var quantity = request.Quantity;
        var country = request.Country;
        var couponCode = request.CouponCode;

        EnsureOnNewOrderPage();

        _newOrderPage!.InputSku(sku);
        _newOrderPage.InputQuantity(quantity);
        _newOrderPage.InputCountry(country);
        _newOrderPage.InputCouponCode(couponCode);
        _newOrderPage.ClickPlaceOrder();

        var result = _newOrderPage.GetResult();
        if (result.IsFailure)
        {
            return Failure<PlaceOrderResponse>(result.Error);
        }

        var orderNumberValue = NewOrderPage.GetOrderNumber(result.Value);
        var response = new PlaceOrderResponse
        {
            OrderNumber = orderNumberValue
        };

        return Success(response);
    }

    public Result<VoidValue, SystemError> CancelOrder(string? orderNumber)
    {
        var result = EnsureOnOrderDetailsPage(orderNumber);
        if (result.IsFailure)
        {
            return result.MapVoid();
        }

        _orderDetailsPage!.ClickCancelOrder();

        var cancelResult = _orderDetailsPage.GetResult();
        if (cancelResult.IsFailure)
        {
            return Failure<VoidValue>(cancelResult.Error);
        }

        var successMessage = cancelResult.Value;
        if (!successMessage.Contains("cancelled successfully!"))
        {
            return Failure<VoidValue>("Did not receive expected cancellation success message");
        }

        var displayStatusAfterCancel = _orderDetailsPage.GetStatus();
        if (displayStatusAfterCancel != OrderStatus.Cancelled)
        {
            return Failure<VoidValue>("Order status not updated to CANCELLED");
        }

        if (!_orderDetailsPage.IsCancelButtonHidden())
        {
            return Failure<VoidValue>("Cancel button still visible");
        }

        return Success();
    }

    public Result<ViewOrderResponse, SystemError> ViewOrder(string? orderNumber)
    {
        var result = EnsureOnOrderDetailsPage(orderNumber);
        if (result.IsFailure)
        {
            return Failure<ViewOrderResponse>(result.Error);
        }

        var isSuccess = _orderDetailsPage!.IsLoadedSuccessfully();
        if (!isSuccess)
        {
            return Failure<ViewOrderResponse>(result.Error);
        }

        var displayOrderNumber = _orderDetailsPage.GetOrderNumber();
        var sku = _orderDetailsPage.GetSku();
        var quantity = _orderDetailsPage.GetQuantity();
        var country = _orderDetailsPage.GetCountry();
        var unitPrice = _orderDetailsPage.GetUnitPrice();
        var basePrice = _orderDetailsPage.GetBasePrice();
        var discountRate = _orderDetailsPage.GetDiscountRate();
        var discountAmount = _orderDetailsPage.GetDiscountAmount();
        var subtotalPrice = _orderDetailsPage.GetSubtotalPrice();
        var taxRate = _orderDetailsPage.GetTaxRate();
        var taxAmount = _orderDetailsPage.GetTaxAmount();
        var totalPrice = _orderDetailsPage.GetTotalPrice();
        var status = _orderDetailsPage.GetStatus();
        var appliedCouponCode = _orderDetailsPage.GetAppliedCoupon();

        var response = new ViewOrderResponse
        {
            OrderNumber = displayOrderNumber,
            OrderTimestamp = DateTime.Now, // TODO: VJ: Actually implement this
            Sku = sku,
            Quantity = quantity,
            Country = country,
            UnitPrice = unitPrice,
            BasePrice = basePrice,
            DiscountRate = discountRate,
            DiscountAmount = discountAmount,
            SubtotalPrice = subtotalPrice,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            TotalPrice = totalPrice,
            Status = status,
            AppliedCouponCode = appliedCouponCode
        };

        return Success(response);
    }

    private void EnsureOnNewOrderPage()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.NEW_ORDER))
        {
            var homePage = _homePageSupplier();
            _newOrderPage = homePage.ClickNewOrder();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.NEW_ORDER);
        }
    }

    private void EnsureOnOrderHistoryPage()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.ORDER_HISTORY))
        {
            var homePage = _homePageSupplier();
            _orderHistoryPage = homePage.ClickOrderHistory();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.ORDER_HISTORY);
        }
    }

    private Result<VoidValue, SystemError> EnsureOnOrderDetailsPage(string? orderNumber)
    {
        EnsureOnOrderHistoryPage();
        
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage.ClickSearch();
        
        var isOrderListed = _orderHistoryPage.IsOrderListed(orderNumber);
        if (!isOrderListed)
        {
            return Failure<VoidValue>("Order " + orderNumber + " does not exist.");
        }
        
        _orderDetailsPage = _orderHistoryPage.ClickViewOrderDetails(orderNumber);
        _pageNavigator.SetCurrentPage(PageNavigator.Page.ORDER_DETAILS);
        
        return Success();
    }
}