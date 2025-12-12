using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;
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

    public Result<VoidValue> GoToShop()
    {
        _homePage = _client.OpenHomePage();

        var statusResult = _client.CheckStatusOk();

        if (statusResult.IsFailure()) {
            return statusResult;
        }

        var pageLoadedResult = _client.CheckPageLoaded();

        if (pageLoadedResult.IsFailure())
        {
            return pageLoadedResult;
        }

        _currentPage = Pages.Home;
        return Result.Success();
    }

    public Result<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request)
    {
        EnsureOnNewOrderPage();
        _newOrderPage!.InputSku(request.Sku);
        _newOrderPage!.InputQuantity(request.Quantity);
        _newOrderPage!.InputCountry(request.Country);
        _newOrderPage!.ClickPlaceOrder();

        var isSuccess = _newOrderPage.HasSuccessNotification();

        if (!isSuccess)
        {
            var errorMessage = _newOrderPage.ReadErrorNotification();
            return Result<PlaceOrderResponse>.FailureResult(errorMessage);
        }

        var orderNumberValue = _newOrderPage.GetOrderNumber();
        var response = new PlaceOrderResponse { OrderNumber = orderNumberValue };
        return Result<PlaceOrderResponse>.SuccessResult(response);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage!.ClickSearch();

        var isSuccess = _orderHistoryPage.HasOrderDetails();

        if (!isSuccess)
        {
            var errorMessage = _orderHistoryPage.ReadErrorNotification();
            return Result<GetOrderResponse>.FailureResult(errorMessage);
        }

        var displayOrderNumber = _orderHistoryPage.GetOrderNumber();
        var sku = _orderHistoryPage.GetSku();
        var quantity = _orderHistoryPage.GetQuantity();
        var country = _orderHistoryPage.GetCountry();
        var unitPrice = _orderHistoryPage.GetUnitPrice();
        var originalPrice = _orderHistoryPage.GetOriginalPrice();
        var discountRate = _orderHistoryPage.GetDiscountRate();
        var discountAmount = _orderHistoryPage.GetDiscountAmount();
        var subtotalPrice = _orderHistoryPage.GetSubtotalPrice();
        var taxRate = _orderHistoryPage.GetTaxRate();
        var taxAmount = _orderHistoryPage.GetTaxAmount();
        var totalPrice = _orderHistoryPage.GetTotalPrice();
        var status = _orderHistoryPage.GetStatus();

        var response = new GetOrderResponse
        {
            OrderNumber = displayOrderNumber,
            Sku = sku,
            Quantity = quantity,
            UnitPrice = unitPrice,
            OriginalPrice = originalPrice,
            DiscountRate = discountRate,
            DiscountAmount = discountAmount,
            SubtotalPrice = subtotalPrice,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            TotalPrice = totalPrice,
            Country = country,
            Status = status
        };

        return Result<GetOrderResponse>.SuccessResult(response);
    }

    public Result<VoidValue> CancelOrder(string orderNumberAlias)
    {
        ViewOrder(orderNumberAlias);
        _orderHistoryPage!.ClickCancelOrder();

        var cancellationMessage = _orderHistoryPage.ReadSuccessNotification();
        if (cancellationMessage != "Order cancelled successfully!")
        {
            return Result.Failure("Did not see cancellation success message, instead: " + cancellationMessage);
        }

        var displayStatusAfterCancel = _orderHistoryPage.GetStatus();
        if (displayStatusAfterCancel != OrderStatus.CANCELLED)
        {
            return Result.Failure("Did not see cancelled status, instead: " + displayStatusAfterCancel);
        }

        if(!_orderHistoryPage.IsCancelButtonHidden()) {
            return Result.Failure("Cancel button is not hidden");
        }

        return Result.Success();
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
        if (_currentPage != Pages.OrderHistory)
        {
            _homePage = _client.OpenHomePage();
            _orderHistoryPage = _homePage.ClickOrderHistory();
            _currentPage = Pages.OrderHistory;
        }
    }
}
