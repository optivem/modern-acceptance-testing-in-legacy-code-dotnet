using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Dtos;
using Optivem.EShop.SystemTest.Core.Clients.System.Api.Dtos.Enums;

namespace Optivem.EShop.SystemTest.E2eTests;

public class ApiE2eTest : IAsyncLifetime
{
    private ShopApiClient? _shopApiClient;
    private ErpApiClient? _erpApiClient;

    public Task InitializeAsync()
    {
        _shopApiClient = ClientFactory.CreateShopApiClient();
        _erpApiClient = ClientFactory.CreateErpApiClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_shopApiClient);
        await ClientCloser.CloseAsync(_erpApiClient);
    }

    [Fact]
    public async Task PlaceOrder_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var baseSku = "AUTO-PO-100";
        var unitPrice = 99.99m;
        var quantity = 5;

        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, unitPrice);

        // Act
        var httpResponse = await _shopApiClient!.Orders().PlaceOrderAsync(sku, quantity.ToString(), "US");

        // Assert
        var response = await _shopApiClient.Orders().AssertOrderPlacedSuccessfullyAsync(httpResponse);
        Assert.NotNull(response.OrderNumber);
        Assert.StartsWith("ORD-", response.OrderNumber);
    }

    [Fact]
    public async Task GetOrder_WithExistingOrder_ShouldReturnOrder()
    {
        // Arrange
        var baseSku = "AUTO-GO-200";
        var unitPrice = 125.50m;
        var quantity = 2;
        var country = "US";

        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, unitPrice);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, quantity, country);

        // Act
        var httpResponse = await _shopApiClient!.Orders().ViewOrderAsync(orderNumber);

        // Assert
        var order = await _shopApiClient.Orders().AssertOrderViewedSuccessfullyAsync(httpResponse);
        Assert.Equal(orderNumber, order.OrderNumber);
        Assert.Equal(sku, order.Sku);
        Assert.Equal(quantity, order.Quantity);
        Assert.Equal(country, order.Country);
        Assert.Equal(unitPrice, order.UnitPrice);
        Assert.Equal(251.00m, order.OriginalPrice);
        Assert.Equal(OrderStatus.PLACED, order.Status);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        var orderNumber = "NON-EXISTENT-ORDER";

        // Act
        var httpResponse = await _shopApiClient!.Orders().ViewOrderAsync(orderNumber);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, httpResponse.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithExistingPlacedOrder_ShouldReturnNoContent()
    {
        // Arrange
        var baseSku = "AUTO-CO-300";
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, 99.99m);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, 2, "US");

        // Act
        var cancelResponse = await _shopApiClient!.Orders().CancelOrderAsync(orderNumber);

        // Assert
        _shopApiClient.Orders().AssertOrderCancelledSuccessfully(cancelResponse);
        
        // Verify order is cancelled
        var order = await GetOrderDetailsAsync(orderNumber);
        Assert.Equal(OrderStatus.CANCELLED, order.Status);
    }

    [Fact]
    public async Task CancelOrder_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        var orderNumber = "NON-EXISTENT-ORDER";

        // Act
        var httpResponse = await _shopApiClient!.Orders().CancelOrderAsync(orderNumber);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, httpResponse.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithAlreadyCancelledOrder_ShouldReturnUnprocessableEntity()
    {
        // Arrange
        var baseSku = "AUTO-CC-400";
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, 89.99m);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, 1, "US");
        await _shopApiClient!.Orders().CancelOrderAsync(orderNumber);

        // Act
        var httpResponse = await _shopApiClient.Orders().CancelOrderAsync(orderNumber);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, httpResponse.StatusCode);
    }

    [Theory]
    [InlineData("", "5", "US", "SKU must not be empty", false)]
    [InlineData("AUTO-IV-300", "0", "US", "Quantity must be positive", true)]
    [InlineData("AUTO-NQ-400", "-1", "US", "Quantity must be positive", true)]
    [InlineData("AUTO-IV-500", "5", "", "Country must not be empty", true)]
    public async Task PlaceOrder_WithInvalidRequest_ShouldReturnBadRequest(
        string sku, string quantity, string country, string expectedError, bool setupErpProduct)
    {
        // Arrange
        var actualSku = sku;
        if (setupErpProduct && !string.IsNullOrEmpty(sku))
        {
            actualSku = await _erpApiClient!.Products().CreateProductAsync(sku, 99.99m);
        }

        // Act
        var httpResponse = await _shopApiClient!.Orders().PlaceOrderAsync(actualSku, quantity, country);

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuantityTestData))]
    public async Task PlaceOrder_WithInvalidQuantityType_ShouldReturnBadRequest(
        object quantity, string expectedError)
    {
        // Arrange
        var baseSku = "AUTO-EQ-500";
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, 150.00m);

        // Act
        var httpResponse = await _shopApiClient!.Orders().PlaceOrderAsync(sku, quantity?.ToString() ?? "", "US");

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    public static IEnumerable<object[]> GetInvalidQuantityTestData()
    {
        yield return new object[] { null!, "Quantity must not be empty" };
    }

    [Fact]
    public async Task PlaceOrder_WithMissingFields_ShouldReturnBadRequest()
    {
        // Act
        var httpResponse = await _shopApiClient!.Orders().PlaceOrderAsync("", "", "");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, httpResponse.StatusCode);
    }

    private async Task<string> PlaceOrderAndGetOrderNumberAsync(string sku, int quantity, string country)
    {
        var httpResponse = await _shopApiClient!.Orders().PlaceOrderAsync(sku, quantity.ToString(), country);
        var placeOrderResponse = await _shopApiClient.Orders().AssertOrderPlacedSuccessfullyAsync(httpResponse);
        return placeOrderResponse.OrderNumber!;
    }

    private async Task<GetOrderResponse> GetOrderDetailsAsync(string orderNumber)
    {
        var httpResponse = await _shopApiClient!.Orders().ViewOrderAsync(orderNumber);
        return await _shopApiClient.Orders().AssertOrderViewedSuccessfullyAsync(httpResponse);
    }
}
