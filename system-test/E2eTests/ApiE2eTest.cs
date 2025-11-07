using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public class ApiE2eTest
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task PlaceOrder_ShouldReturnOrderNumber()
    {
        // Arrange
        var request = new PlaceOrderRequest
        {
            ProductId = 10,
            Quantity = 5
        };

        using var client = new HttpClient();
        
        // Act
        var response = await client.PostAsJsonAsync($"{TestConfiguration.BaseUrl}/api/orders", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var orderResponse = JsonSerializer.Deserialize<PlaceOrderResponse>(responseBody, JsonOptions);
        
        Assert.NotNull(orderResponse);
        Assert.NotNull(orderResponse.OrderNumber);
        Assert.True(orderResponse.OrderNumber.StartsWith("ORD-"), "Order number should start with ORD-");
    }

    [Fact]
    public async Task GetOrder_ShouldReturnOrderDetails()
    {
        // Arrange - First place an order
        var placeOrderRequest = new PlaceOrderRequest
        {
            ProductId = 11,
            Quantity = 3
        };

        using var client = new HttpClient();
        var postResponse = await client.PostAsJsonAsync($"{TestConfiguration.BaseUrl}/api/orders", placeOrderRequest);
        var postBody = await postResponse.Content.ReadAsStringAsync();
        var placeOrderResponse = JsonSerializer.Deserialize<PlaceOrderResponse>(postBody, JsonOptions);
        var orderNumber = placeOrderResponse!.OrderNumber;
        
        // Act - Get the order details
        var getResponse = await client.GetAsync($"{TestConfiguration.BaseUrl}/api/orders/{orderNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var getBody = await getResponse.Content.ReadAsStringAsync();
        var getOrderResponse = JsonSerializer.Deserialize<GetOrderResponse>(getBody, JsonOptions);
        
        Assert.NotNull(getOrderResponse);
        Assert.Equal(orderNumber, getOrderResponse.OrderNumber);
        Assert.Equal(11L, getOrderResponse.ProductId);
        Assert.Equal(3, getOrderResponse.Quantity);
        
        // Price will come from DummyJSON API for product 11
        Assert.True(getOrderResponse.UnitPrice > 0, "Unit price should be positive");
        Assert.True(getOrderResponse.TotalPrice > 0, "Total price should be positive");
    }

    [Theory]
    [InlineData(11L, 3)]   // Product 11 with standard quantity
    [InlineData(12L, 5)]   // Product 12 with medium quantity
    [InlineData(13L, 1)]   // Product 13 with minimum quantity
    [InlineData(14L, 10)]  // Product 14 with large quantity
    public async Task GetOrder_ShouldReturnOrderDetails_WithMultipleProducts(long productId, int quantity)
    {
        // Arrange - First place an order
        var placeOrderRequest = new PlaceOrderRequest
        {
            ProductId = productId,
            Quantity = quantity
        };

        using var client = new HttpClient();
        var postResponse = await client.PostAsJsonAsync($"{TestConfiguration.BaseUrl}/api/orders", placeOrderRequest);
        var postBody = await postResponse.Content.ReadAsStringAsync();
        var placeOrderResponse = JsonSerializer.Deserialize<PlaceOrderResponse>(postBody, JsonOptions);
        var orderNumber = placeOrderResponse!.OrderNumber;
        
        // Act - Get the order details
        var getResponse = await client.GetAsync($"{TestConfiguration.BaseUrl}/api/orders/{orderNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var getBody = await getResponse.Content.ReadAsStringAsync();
        var getOrderResponse = JsonSerializer.Deserialize<GetOrderResponse>(getBody, JsonOptions);
        
        Assert.NotNull(getOrderResponse);
        Assert.Equal(orderNumber, getOrderResponse.OrderNumber);
        Assert.Equal(productId, getOrderResponse.ProductId);
        Assert.Equal(quantity, getOrderResponse.Quantity);
        
        // Price will come from DummyJSON API for product
        Assert.NotNull(getOrderResponse.UnitPrice);
        Assert.NotNull(getOrderResponse.TotalPrice);
    }

    [Theory]
    [InlineData("10.5")]  // Decimal value
    [InlineData("xyz")]   // Non-numeric string
    public async Task PlaceOrder_ShouldRejectNonIntegerProductId(string invalidProductId)
    {
        // Arrange
        var jsonContent = $$"""
            {
                "productId": {{invalidProductId}},
                "quantity": 5
            }
            """;
        
        using var client = new HttpClient();
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        
        // Act
        var response = await client.PostAsync($"{TestConfiguration.BaseUrl}/api/orders", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Product ID must be an integer", responseBody);
    }

    [Theory]
    [InlineData("3.5")]   // Decimal value
    [InlineData("lala")]  // Non-numeric string
    public async Task PlaceOrder_ShouldRejectNonIntegerQuantity(string invalidQuantity)
    {
        // Arrange
        var jsonContent = $$"""
            {
                "productId": 10,
                "quantity": {{invalidQuantity}}
            }
            """;
        
        using var client = new HttpClient();
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        
        // Act
        var response = await client.PostAsync($"{TestConfiguration.BaseUrl}/api/orders", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Quantity must be an integer", responseBody);
    }

    [Fact]
    public async Task CancelOrder_ShouldSetStatusToCancelled()
    {
        // Arrange - First place an order
        var placeOrderRequest = new PlaceOrderRequest
        {
            ProductId = 12,
            Quantity = 2
        };

        using var client = new HttpClient();
        var postResponse = await client.PostAsJsonAsync($"{TestConfiguration.BaseUrl}/api/orders", placeOrderRequest);
        var postBody = await postResponse.Content.ReadAsStringAsync();
        var placeOrderResponse = JsonSerializer.Deserialize<PlaceOrderResponse>(postBody, JsonOptions);
        var orderNumber = placeOrderResponse!.OrderNumber;
        
        // Act - Cancel the order
        var deleteResponse = await client.DeleteAsync($"{TestConfiguration.BaseUrl}/api/orders/{orderNumber}");

        // Assert - Verify cancel response
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        // Verify order status is CANCELLED
        var getResponse = await client.GetAsync($"{TestConfiguration.BaseUrl}/api/orders/{orderNumber}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var getBody = await getResponse.Content.ReadAsStringAsync();
        var getOrderResponse = JsonSerializer.Deserialize<GetOrderResponse>(getBody, JsonOptions);
        
        Assert.NotNull(getOrderResponse);
        Assert.Equal("Cancelled", getOrderResponse.Status);
    }

    private class PlaceOrderRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
    
    private class PlaceOrderResponse
    {
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }
    
    private class GetOrderResponse
    {
        public string OrderNumber { get; set; } = string.Empty;
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}