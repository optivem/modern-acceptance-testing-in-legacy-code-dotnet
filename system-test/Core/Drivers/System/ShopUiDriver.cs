using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos.Enums;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

public class ShopUiDriver : IShopDriver
{
    private readonly ShopUiClient _client;

    private HomePage? _homePage;
    private NewOrderPage? _newOrderPage;
    private OrderHistoryPage? _orderHistoryPage;

    private Pages _currentPage;

    private enum Pages
    {
        Home,
        NewOrder,
        OrderHistory
    }

    public ShopUiDriver(string baseUrl)
    {
        _client = new ShopUiClient(baseUrl);
    }

    public Result<object?> GoToShop()
    {
        _homePage = _client.OpenHomePage();
        _currentPage = Pages.Home;

        try
        {
            _client.AssertPageLoaded();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure<object?>(new[] { ex.Message });
        }
    }

    public Result<PlaceOrderResponse> PlaceOrder(string? sku, string? quantity, string? country)
    {
        EnsureOnNewOrderPage();
        _newOrderPage!.InputSku(sku ?? string.Empty);
        _newOrderPage.InputQuantity(quantity ?? string.Empty);
        _newOrderPage.InputCountry(country ?? string.Empty);
        _newOrderPage.ClickPlaceOrder();

        var hasSuccess = _newOrderPage.HasSuccessNotification();

        if (!hasSuccess)
        {
            var errorMessage = _newOrderPage.ReadErrorNotification();
            return Result.Failure<PlaceOrderResponse>(new[] { errorMessage });
        }

        var orderNumber = _newOrderPage.GetOrderNumber();
        var response = new PlaceOrderResponse { OrderNumber = orderNumber };
        return Result.Success(response);
    }

    public Result<GetOrderResponse> ViewOrder(string orderNumber)
    {
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage.ClickSearch();

        var isSuccess = _orderHistoryPage.HasOrderDetails();

        if (!isSuccess)
        {
            var errorMessage = _orderHistoryPage.ReadErrorNotification();
            return Result.Failure<GetOrderResponse>(new[] { errorMessage });
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
            Quantity = int.Parse(quantity),
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

        return Result.Success(response);
    }

    public Result<object?> CancelOrder(string orderNumber)
    {
        EnsureOnOrderHistoryPage();
        _orderHistoryPage!.InputOrderNumber(orderNumber);
        _orderHistoryPage.ClickSearch();

        var hasOrderDetails = _orderHistoryPage.HasOrderDetails();

        if (!hasOrderDetails)
        {
            var errorMessage = _orderHistoryPage.ReadErrorNotification();
            return Result.Failure<object?>(new[] { errorMessage });
        }

        _orderHistoryPage.ClickCancelOrder();

        var hasSuccess = _orderHistoryPage.HasSuccessNotification();

        if (!hasSuccess)
        {
            var errorMessage = _orderHistoryPage.ReadErrorNotification();
            return Result.Failure<object?>(new[] { errorMessage });
        }

        return Result.Success();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private void EnsureOnNewOrderPage()
    {
        if (_currentPage == Pages.NewOrder)
        {
            return;
        }

        if (_currentPage != Pages.Home)
        {
            throw new InvalidOperationException("Cannot navigate to New Order page from current page");
        }

        _newOrderPage = _homePage!.ClickNewOrder();
        _currentPage = Pages.NewOrder;
    }

    private void EnsureOnOrderHistoryPage()
    {
        if (_currentPage == Pages.OrderHistory)
        {
            return;
        }

        if (_currentPage != Pages.Home)
        {
            throw new InvalidOperationException("Cannot navigate to Order History page from current page");
        }

        _orderHistoryPage = _homePage!.ClickOrderHistory();
        _currentPage = Pages.OrderHistory;
    }
}
