using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Internal;
using static Optivem.EShop.SystemTest.Core.Shop.Commons.SystemResults;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Internal;

public class ShopUiOrderDriver : IOrderDriver
{
    private readonly Func<Task<HomePage>> _homePageSupplier;
    private readonly PageNavigator _pageNavigator;
    
    private NewOrderPage? _newOrderPage;
    private OrderHistoryPage? _orderHistoryPage;
    private OrderDetailsPage? _orderDetailsPage;

    public ShopUiOrderDriver(Func<Task<HomePage>> homePageSupplier, PageNavigator pageNavigator)
    {
        _homePageSupplier = homePageSupplier;
        _pageNavigator = pageNavigator;
    }

    public async Task<Result<PlaceOrderResponse, SystemError>> PlaceOrder(PlaceOrderRequest request)
    {
        var sku = request.Sku;
        var quantity = request.Quantity;
        var country = request.Country;
        var couponCode = request.CouponCode;

        await EnsureOnNewOrderPageAsync();

        await _newOrderPage!.InputSkuAsync(sku);
        await _newOrderPage.InputQuantityAsync(quantity);
        await _newOrderPage.InputCountryAsync(country);
        await _newOrderPage.InputCouponCodeAsync(couponCode);
        await _newOrderPage.ClickPlaceOrderAsync();

        var result = await _newOrderPage.GetResultAsync();
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

    public async Task<Result<ViewOrderResponse, SystemError>> ViewOrder(string? orderNumber)
    {
        var result = await EnsureOnOrderDetailsPageAsync(orderNumber);
        if (result.IsFailure)
        {
            return Failure<ViewOrderResponse>(result.Error);
        }

        var isSuccess = await _orderDetailsPage!.IsLoadedSuccessfullyAsync();
        if (!isSuccess)
        {
            return Failure<ViewOrderResponse>(result.Error);
        }

        var displayOrderNumber = await _orderDetailsPage.GetOrderNumberAsync();
        var orderTimestamp = await _orderDetailsPage.GetOrderTimestampAsync();
        var sku = await _orderDetailsPage.GetSkuAsync();
        var quantity = await _orderDetailsPage.GetQuantityAsync();
        var country = await _orderDetailsPage.GetCountryAsync();
        var unitPrice = await _orderDetailsPage.GetUnitPriceAsync();
        var basePrice = await _orderDetailsPage.GetBasePriceAsync();
        var discountRate = await _orderDetailsPage.GetDiscountRateAsync();
        var discountAmount = await _orderDetailsPage.GetDiscountAmountAsync();
        var subtotalPrice = await _orderDetailsPage.GetSubtotalPriceAsync();
        var taxRate = await _orderDetailsPage.GetTaxRateAsync();
        var taxAmount = await _orderDetailsPage.GetTaxAmountAsync();
        var totalPrice = await _orderDetailsPage.GetTotalPriceAsync();
        var status = await _orderDetailsPage.GetStatusAsync();
        var appliedCouponCode = await _orderDetailsPage.GetAppliedCouponAsync();

        var response = new ViewOrderResponse
        {
            OrderNumber = displayOrderNumber,
            OrderTimestamp = orderTimestamp.DateTime,
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

    public async Task<Result<VoidValue, SystemError>> CancelOrder(string? orderNumber)
    {
        var viewResult = await ViewOrder(orderNumber);
        if (viewResult.IsFailure)
        {
            return viewResult.MapVoid();
        }

        await _orderDetailsPage!.ClickCancelOrderAsync();

        var cancelResult = await _orderDetailsPage.GetResultAsync();
        if (cancelResult.IsFailure)
        {
            return Failure(cancelResult.Error);
        }

        var successMessage = cancelResult.Value;
        if (!successMessage.Contains("cancelled successfully!"))
        {
            return Failure("Did not receive expected cancellation success message");
        }

        var displayStatusAfterCancel = await _orderDetailsPage.GetStatusAsync();
        if (displayStatusAfterCancel != OrderStatus.Cancelled)
        {
            return Failure("Order status not updated to CANCELLED");
        }

        if (!await _orderDetailsPage.IsCancelButtonHiddenAsync())
        {
            return Failure("Cancel button still visible");
        }

        return Success();
    }

    private async Task EnsureOnNewOrderPageAsync()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.NEW_ORDER))
        {
            var homePage = await _homePageSupplier();
            _newOrderPage = await homePage.ClickNewOrderAsync();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.NEW_ORDER);
        }
    }

    private async Task EnsureOnOrderHistoryPageAsync()
    {
        if (!_pageNavigator.IsOnPage(PageNavigator.Page.ORDER_HISTORY))
        {
            var homePage = await _homePageSupplier();
            _orderHistoryPage = await homePage.ClickOrderHistoryAsync();
            _pageNavigator.SetCurrentPage(PageNavigator.Page.ORDER_HISTORY);
        }
    }

    private async Task<Result<VoidValue, SystemError>> EnsureOnOrderDetailsPageAsync(string? orderNumber)
    {
        await EnsureOnOrderHistoryPageAsync();
        
        await _orderHistoryPage!.InputOrderNumberAsync(orderNumber);
        await _orderHistoryPage.ClickSearchAsync();
        
        var isOrderListed = await _orderHistoryPage.IsOrderListedAsync(orderNumber);
        if (!isOrderListed)
        {
            return Failure("Order " + orderNumber + " does not exist.");
        }
        
        _orderDetailsPage = await _orderHistoryPage.ClickViewOrderDetailsAsync(orderNumber);
        _pageNavigator.SetCurrentPage(PageNavigator.Page.ORDER_DETAILS);
        
        return Success();
    }
}