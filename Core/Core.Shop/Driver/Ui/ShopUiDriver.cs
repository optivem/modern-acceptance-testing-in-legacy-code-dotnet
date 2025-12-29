using Optivem.Lang;
using Optivem.Testing.Assertions;
using Optivem.EShop.SystemTest.Core.Common.Error;
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

        var isSuccess = _newOrderPage.HasSuccessNotification();

        if (!isSuccess)
        {
            var generalMessage = _newOrderPage.ReadGeneralErrorMessage();
            var fieldErrorTexts = _newOrderPage.ReadFieldErrors();

            if (fieldErrorTexts.Count == 0)
            {
                // Business logic error - no field errors
                return Results.Failure<PlaceOrderResponse>(generalMessage);
            }
            else
            {
                // Validation error with field errors
                // Parse "fieldName: message" format
                var fieldErrors = fieldErrorTexts
                    .Select(text =>
                    {
                        var parts = text.Split(':', 2);
                        if (parts.Length == 2)
                        {
                            return new Error.FieldError(parts[0].Trim(), parts[1].Trim());
                        }
                        return new Error.FieldError("unknown", text);
                    })
                    .ToList();

                var error = Error.Of(generalMessage, fieldErrors.AsReadOnly());
                return Results.Failure<PlaceOrderResponse>(error);
            }
        }

        var orderNumberValue = _newOrderPage.GetOrderNumber();
        var response = new PlaceOrderResponse { OrderNumber = orderNumberValue };
        return Results.Success(response);
    }

    public Result<GetOrderResponse, Error> ViewOrder(string orderNumber)
    {
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage!.ClickSearch();

        var isSuccess = _orderHistoryPage.HasOrderDetails();

        if (!isSuccess)
        {
            var errorMessages = _orderHistoryPage.ReadErrorNotification();
            var errorMessage = errorMessages.Count > 0 ? errorMessages[0] : "View order failed";
            return Results.Failure<GetOrderResponse>(errorMessage);
        }

        var displayOrderNumber = _orderHistoryPage.GetOrderNumber();
        var sku = _orderHistoryPage.GetSku();
        var quantity = _orderHistoryPage.GetQuantity();
        var country = _orderHistoryPage.GetCountry();
        var unitPrice = _orderHistoryPage.GetUnitPrice();
        var subtotalPrice = _orderHistoryPage.GetSubtotalPrice();
        var discountRate = _orderHistoryPage.GetDiscountRate();
        var discountAmount = _orderHistoryPage.GetDiscountAmount();
        var preTaxTotal = _orderHistoryPage.GetPreTaxTotal();
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
            SubtotalPrice = subtotalPrice,
            DiscountRate = discountRate,
            DiscountAmount = discountAmount,
            PreTaxTotal = preTaxTotal,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            TotalPrice = totalPrice,
            Country = country,
            Status = status
        };

        return Results.Success(response);
    }

    public Result<VoidValue, Error> CancelOrder(string orderNumberAlias)
    {
        ViewOrder(orderNumberAlias);
        _orderHistoryPage!.ClickCancelOrder();

        var cancellationMessage = _orderHistoryPage.ReadSuccessNotification();
        if (cancellationMessage != "Order cancelled successfully!")
        {
            return Results.Failure<VoidValue>("Did not see cancellation success message, instead: " + cancellationMessage);
        }

        var displayStatusAfterCancel = _orderHistoryPage.GetStatus();
        if (displayStatusAfterCancel != OrderStatus.CANCELLED)
        {
            return Results.Failure<VoidValue>("Did not see cancelled status, instead: " + displayStatusAfterCancel);
        }

        if(!_orderHistoryPage.IsCancelButtonHidden()) {
            return Results.Failure<VoidValue>("Cancel button is not hidden");
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
        if (_currentPage != Pages.OrderHistory)
        {
            _homePage = _client.OpenHomePage();
            _orderHistoryPage = _homePage.ClickOrderHistory();
            _currentPage = Pages.OrderHistory;
        }
    }
}
