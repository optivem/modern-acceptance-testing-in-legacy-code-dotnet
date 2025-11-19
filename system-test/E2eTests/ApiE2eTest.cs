using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api.Dtos;
using Optivem.EShop.SystemTest.Core.Clients.System.Api.Dtos.Enums;

namespace Optivem.EShop.SystemTest.E2eTests;

public class ApiE2eTest : IAsyncLifetime
{
    private ShopApiClient _shopApiClient = default!;
    private ErpApiClient _erpApiClient = default!;

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

        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, unitPrice);

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantity.ToString(), "US");

        // Assert
        var response = await _shopApiClient.Orders().AssertOrderPlacedSuccessfullyAsync(httpResponse);
        Assert.NotNull(response.OrderNumber);
        Assert.StartsWith("ORD-", response.OrderNumber);
    }

    [Fact]
    public async Task GetOrder_WithExistingOrder_ShouldReturnOrder()
    {
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-GO-200";
        var unitPrice = 299.50m;
        var quantity = 3;
        var country = "DE";

        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, unitPrice);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, quantity, country);

        // Act
        var httpResponse = await _shopApiClient.Orders().ViewOrderAsync(orderNumber);

        // Assert - Assert all fields from GetOrderResponse
        var order = await _shopApiClient.Orders().AssertOrderViewedSuccessfullyAsync(httpResponse);
        Assert.NotNull(order.OrderNumber);
        Assert.Equal(orderNumber, order.OrderNumber);
        Assert.Equal(sku, order.Sku);
        Assert.Equal(quantity, order.Quantity);
        Assert.Equal(country, order.Country);
        
        // Assert with concrete values based on known input
        Assert.Equal(unitPrice, order.UnitPrice);
        
        var expectedOriginalPrice = 898.50m;
        Assert.Equal(expectedOriginalPrice, order.OriginalPrice);
        
        Assert.NotNull(order.Status);
        Assert.Equal(OrderStatus.PLACED, order.Status);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        var orderNumber = "NON-EXISTENT-ORDER";

        // Act
        var httpResponse = await _shopApiClient.Orders().ViewOrderAsync(orderNumber);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, httpResponse.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithExistingPlacedOrder_ShouldReturnNoContent()
    {
        // Arrange
        var baseSku = "AUTO-CO-300";
        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, 99.99m);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, 2, "US");

        // Act
        var cancelResponse = await _shopApiClient.Orders().CancelOrderAsync(orderNumber);

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
        var httpResponse = await _shopApiClient.Orders().CancelOrderAsync(orderNumber);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, httpResponse.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithAlreadyCancelledOrder_ShouldReturnUnprocessableEntity()
    {
        // Arrange
        var baseSku = "AUTO-CC-400";
        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, 89.99m);

        var orderNumber = await PlaceOrderAndGetOrderNumberAsync(sku, 1, "US");
        await _shopApiClient.Orders().CancelOrderAsync(orderNumber);

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
            actualSku = await _erpApiClient.Products().CreateProductAsync(sku, 99.99m);
        }

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(actualSku, quantity, country);

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    [Fact]
    public async Task PlaceOrder_WithNonExistentSku_ShouldReturnUnprocessableEntity()
    {
        // Arrange
        var sku = "NON-EXISTENT-SKU-12345";
        var quantity = "5";
        var country = "US";

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantity, country);

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains("Product does not exist for SKU", errorMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuantityTestData))]
    public async Task PlaceOrder_WithInvalidQuantityType_ShouldReturnBadRequest(
        object quantity, string expectedError)
    {
        // Arrange
        var baseSku = "AUTO-EQ-500";
        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, 150.00m);

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantity?.ToString() ?? "", "US");

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    public static IEnumerable<object[]> GetInvalidQuantityTestData()
    {
        yield return new object[] { null!, "Quantity must not be empty" };
    }

    [Theory]
    [MemberData(nameof(GetEmptyQuantityTestData))]
    public async Task PlaceOrder_WithEmptyQuantity_ShouldReturnBadRequest(
        string? quantityValue, string expectedError)
    {
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-EQ-500";
        var unitPrice = 150.00m;

        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, unitPrice);

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantityValue ?? "", "US");

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    public static IEnumerable<object[]> GetEmptyQuantityTestData()
    {
        yield return new object?[] { null, "Quantity must not be empty" };
        yield return new object[] { "", "Quantity must not be empty" };
        yield return new object[] { "   ", "Quantity must not be empty" };
    }

    [Theory]
    [MemberData(nameof(GetNonIntegerQuantityTestData))]
    public async Task PlaceOrder_WithNonIntegerQuantity_ShouldReturnBadRequest(
        string quantityValue, string expectedError)
    {
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-NIQ-600";
        var unitPrice = 175.00m;

        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, unitPrice);

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantityValue, "US");

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    public static IEnumerable<object[]> GetNonIntegerQuantityTestData()
    {
        yield return new object[] { "3.5", "Quantity must be an integer" };
        yield return new object[] { "lala", "Quantity must be an integer" };
    }

    [Theory]
    [MemberData(nameof(GetEmptyCountryTestData))]
    public async Task PlaceOrder_WithEmptyCountry_ShouldReturnBadRequest(
        string? countryValue, string expectedError)
    {
        // Arrange - Set up product in ERP first and get unique SKU
        var baseSku = "AUTO-EC-700";
        var unitPrice = 225.00m;

        var sku = await _erpApiClient.Products().CreateProductAsync(baseSku, unitPrice);

        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, "5", countryValue ?? "");

        // Assert
        _shopApiClient.Orders().AssertOrderPlacementFailed(httpResponse);
        var errorMessage = await _shopApiClient.Orders().GetErrorMessageAsync(httpResponse);
        Assert.Contains(expectedError, errorMessage);
    }

    public static IEnumerable<object[]> GetEmptyCountryTestData()
    {
        yield return new object?[] { null, "Country must not be empty" };
        yield return new object[] { "", "Country must not be empty" };
        yield return new object[] { "   ", "Country must not be empty" };
    }

    [Fact]
    public async Task PlaceOrder_WithMissingFields_ShouldReturnBadRequest()
    {
        // Act
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync("", "", "");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, httpResponse.StatusCode);
    }

    private async Task<string> PlaceOrderAndGetOrderNumberAsync(string sku, int quantity, string country)
    {
        var httpResponse = await _shopApiClient.Orders().PlaceOrderAsync(sku, quantity.ToString(), country);
        var placeOrderResponse = await _shopApiClient.Orders().AssertOrderPlacedSuccessfullyAsync(httpResponse);
        return placeOrderResponse.OrderNumber!;
    }

    private async Task<GetOrderResponse> GetOrderDetailsAsync(string orderNumber)
    {
        var httpResponse = await _shopApiClient.Orders().ViewOrderAsync(orderNumber);
        return await _shopApiClient.Orders().AssertOrderViewedSuccessfullyAsync(httpResponse);
    }
}
