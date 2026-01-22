using Optivem.Commons.Util;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui;

public class ShopUiDriver : IShopDriver
{
    private readonly ShopUiClient _client;

    private HomePage? _homePage;
    private NewOrderPage? _newOrderPage;
    private OrderHistoryPage? _orderHistoryPage;

    private Pages _currentPage;

    private enum Pages
    {
        None,
        Home,
        NewOrder,
        OrderHistory
    }

    public ShopUiDriver(string baseUrl)
    {
        _client = new ShopUiClient(baseUrl);
    }

    public Result<VoidValue, Error> GoToShop()
    {
        _homePage = _client.OpenHomePage();

        var statusResult = _client.CheckStatusOk();

        if (statusResult.IsFailure) {
            return statusResult;
        }

        var pageLoadedResult = _client.CheckPageLoaded();

        if (pageLoadedResult.IsFailure)
        {
            return pageLoadedResult;
        }

        _currentPage = Pages.Home;
        return Results.Success();
    }

    public Result<PlaceOrderResponse, Error> PlaceOrder(PlaceOrderRequest request)
    {
        EnsureOnNewOrderPage();
        _newOrderPage!.InputSku(request.Sku);
        _newOrderPage!.InputQuantity(request.Quantity);
        _newOrderPage!.InputCountry(request.Country);
        _newOrderPage!.ClickPlaceOrder();

        var result = _newOrderPage.GetResult();

        if (result.IsFailure)
        {
            return Results.Failure<PlaceOrderResponse>(result.Error);
        }

        var orderNumberValue = NewOrderPage.ExtractOrderNumber(result.Value);
        var response = new PlaceOrderResponse { OrderNumber = orderNumberValue };
        
        // Reset page state to ensure clean navigation for subsequent operations
        _currentPage = Pages.None;
        
        return Results.Success(response);
    }

    public Result<GetOrderResponse, Error> ViewOrder(string orderNumber)
    {
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage!.ClickSearch();

        var isOrderListed = _orderHistoryPage.IsOrderListed(orderNumber);

        if (!isOrderListed)
        {
            return Results.Failure<GetOrderResponse>(Error.Of($"Order {orderNumber} does not exist."));
        }

        // Navigate to order details page
        var orderDetailsPage = _orderHistoryPage.ClickViewOrderDetails(orderNumber);
        
        // Verify order details page loaded successfully
        var isLoadedSuccessfully = orderDetailsPage.IsLoadedSuccessfully();
        
        if (!isLoadedSuccessfully)
        {
            return Results.Failure<GetOrderResponse>(Error.Of("Failed to load order details page"));
        }

        // Read order details from the details page
        var displayOrderNumber = orderDetailsPage.GetOrderNumber();
        var sku = orderDetailsPage.GetSku();
        var quantity = orderDetailsPage.GetQuantity();
        var country = orderDetailsPage.GetCountry();
        var unitPrice = orderDetailsPage.GetUnitPrice();
        var basePrice = orderDetailsPage.GetBasePrice();
        var subtotalPrice = orderDetailsPage.GetSubtotalPrice();
        var discountRate = orderDetailsPage.GetDiscountRate();
        var discountAmount = orderDetailsPage.GetDiscountAmount();
        var taxRate = orderDetailsPage.GetTaxRate();
        var taxAmount = orderDetailsPage.GetTaxAmount();
        var totalPrice = orderDetailsPage.GetTotalPrice();
        var status = orderDetailsPage.GetStatus();

        var response = new GetOrderResponse
        {
            OrderNumber = displayOrderNumber,
            Sku = sku,
            Quantity = quantity,
            UnitPrice = unitPrice,
            BasePrice = basePrice,
            SubtotalPrice = subtotalPrice,
            DiscountRate = discountRate,
            DiscountAmount = discountAmount,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            TotalPrice = totalPrice,
            Country = country,
            Status = status
        };

        return Results.Success(response);
    }

    public Result<VoidValue, Error> CancelOrder(string orderNumber)
    {
        // Navigate to order details page first
        var viewResult = ViewOrder(orderNumber);
        if (viewResult.IsFailure)
        {
            return Results.Failure<VoidValue>(viewResult.Error.Message);
        }

        // Now we should be on the order details page, so get a reference to it
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage!.ClickSearch();
        var orderDetailsPage = _orderHistoryPage.ClickViewOrderDetails(orderNumber);
        
        // Perform the cancel action
        orderDetailsPage.ClickCancelOrder();

        var result = orderDetailsPage.GetResult();
        
        if (result.IsFailure)
        {
            return Results.Failure<VoidValue>(result.Error.Message);
        }

        var cancellationMessage = result.Value;
        if (cancellationMessage != "Order cancelled successfully!")
        {
            return Results.Failure<VoidValue>("Did not receive expected cancellation success message");
        }

        var displayStatusAfterCancel = orderDetailsPage.GetStatus();
        if (displayStatusAfterCancel != OrderStatus.CANCELLED)
        {
            return Results.Failure<VoidValue>("Order status not updated to CANCELLED");
        }

        if (!orderDetailsPage.IsCancelButtonHidden())
        {
            return Results.Failure<VoidValue>("Cancel button still visible");
        }

        return Results.Success();
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    private void EnsureOnNewOrderPage()
    {
        if (_currentPage != Pages.NewOrder)
        {
            _homePage = _client.OpenHomePage();
            _newOrderPage = _homePage.ClickNewOrder();
            _currentPage = Pages.NewOrder;
        }
    }

    private void EnsureOnOrderHistoryPage()
    {
        // Always refresh to ensure clean navigation, especially after PlaceOrder
        _homePage = _client.OpenHomePage();
        _orderHistoryPage = _homePage.ClickOrderHistory();
        _currentPage = Pages.OrderHistory;
    }
}
