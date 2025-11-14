using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public class ApiE2eTest : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestConfiguration _config;

    public ApiE2eTest()
    {
        _config = new TestConfiguration();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config.BaseUrl)
        };
    }

    [Fact]
    public async Task PlaceOrder_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var request = new
        {
            sku = "WIDGET-001",
            quantity = 5,
            country = "US"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/orders", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var responseBody = await response.Content.ReadFromJsonAsync<JsonElement>();
        var orderNumber = responseBody.GetProperty("orderNumber").GetString();
        Assert.NotNull(orderNumber);
        Assert.NotEmpty(orderNumber);
        
        var location = response.Headers.Location?.ToString();
        Assert.NotNull(location);
        Assert.Contains($"/api/orders/{orderNumber}", location);
    }

    [Fact]
    public async Task GetOrder_WithExistingOrder_ShouldReturnOrder()
    {
        // Arrange - First place an order
        var placeRequest = new
        {
            sku = "WIDGET-002",
            quantity = 3,
            country = "US"
        };
        var placeResponse = await _httpClient.PostAsJsonAsync("/api/orders", placeRequest);
        var placeBody = await placeResponse.Content.ReadFromJsonAsync<JsonElement>();
        var orderNumber = placeBody.GetProperty("orderNumber").GetString();

        // Act
        var getResponse = await _httpClient.GetAsync($"/api/orders/{orderNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var order = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(orderNumber, order.GetProperty("orderNumber").GetString());
        Assert.Equal("WIDGET-002", order.GetProperty("sku").GetString());
        Assert.Equal(3, order.GetProperty("quantity").GetInt32());
        Assert.Equal("US", order.GetProperty("country").GetString());
        Assert.Equal("PLACED", order.GetProperty("status").GetString());
        
        // Verify price calculations
        Assert.True(order.GetProperty("unitPrice").GetDecimal() > 0);
        Assert.True(order.GetProperty("originalPrice").GetDecimal() > 0);
        Assert.True(order.GetProperty("subtotalPrice").GetDecimal() > 0);
        Assert.True(order.GetProperty("totalPrice").GetDecimal() > 0);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        var orderNumber = "NON-EXISTENT-ORDER";

        // Act
        var response = await _httpClient.GetAsync($"/api/orders/{orderNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithExistingPlacedOrder_ShouldReturnNoContent()
    {
        // Arrange - First place an order
        var placeRequest = new
        {
            sku = "WIDGET-003",
            quantity = 2,
            country = "US"
        };
        var placeResponse = await _httpClient.PostAsJsonAsync("/api/orders", placeRequest);
        var placeBody = await placeResponse.Content.ReadFromJsonAsync<JsonElement>();
        var orderNumber = placeBody.GetProperty("orderNumber").GetString();

        // Act
        var cancelResponse = await _httpClient.PostAsync($"/api/orders/{orderNumber}/cancel", null);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, cancelResponse.StatusCode);
        
        // Verify order is cancelled
        var getResponse = await _httpClient.GetAsync($"/api/orders/{orderNumber}");
        var order = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("CANCELLED", order.GetProperty("status").GetString());
    }

    [Fact]
    public async Task CancelOrder_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        var orderNumber = "NON-EXISTENT-ORDER";

        // Act
        var response = await _httpClient.PostAsync($"/api/orders/{orderNumber}/cancel", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_WithAlreadyCancelledOrder_ShouldReturnBadRequest()
    {
        // Arrange - First place and cancel an order
        var placeRequest = new
        {
            sku = "WIDGET-004",
            quantity = 1,
            country = "US"
        };
        var placeResponse = await _httpClient.PostAsJsonAsync("/api/orders", placeRequest);
        var placeBody = await placeResponse.Content.ReadFromJsonAsync<JsonElement>();
        var orderNumber = placeBody.GetProperty("orderNumber").GetString();
        
        await _httpClient.PostAsync($"/api/orders/{orderNumber}/cancel", null);

        // Act - Try to cancel again
        var response = await _httpClient.PostAsync($"/api/orders/{orderNumber}/cancel", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("", 5, "US", "SKU must not be empty")]
    [InlineData("WIDGET-005", 0, "US", "Quantity must be positive")]
    [InlineData("WIDGET-006", -1, "US", "Quantity must be positive")]
    [InlineData("WIDGET-007", 5, "", "Country must not be empty")]
    public async Task PlaceOrder_WithInvalidRequest_ShouldReturnBadRequest(
        string sku, int quantity, string country, string expectedError)
    {
        // Arrange
        var request = new Dictionary<string, object?>
        {
            ["sku"] = string.IsNullOrEmpty(sku) ? null : sku,
            ["quantity"] = quantity,
            ["country"] = string.IsNullOrEmpty(country) ? null : country
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/orders", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedError, responseBody);
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuantityTestData))]
    public async Task PlaceOrder_WithInvalidQuantityType_ShouldReturnBadRequest(
        object quantity, string expectedError)
    {
        // Arrange
        var request = new Dictionary<string, object?>
        {
            ["sku"] = "WIDGET-008",
            ["quantity"] = quantity,
            ["country"] = "US"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/orders", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedError, responseBody);
    }

    public static IEnumerable<object[]> GetInvalidQuantityTestData()
    {
        yield return new object[] { null!, "Quantity must not be empty" };
    }

    [Fact]
    public async Task PlaceOrder_WithMissingFields_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new { };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/orders", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private record ErpProduct(
        string Id,
        string Title,
        string Description,
        decimal Price,
        string Category,
        string Brand
    );

    private async Task SetupProductInErp(
        string sku,
        string title,
        decimal price,
        string description = "Test product description",
        string category = "Test Category",
        string brand = "Test Brand")
    {
        var erpApiUrl = _config.BaseUrl.Replace(":8081", ":3100");
        using var erpClient = new HttpClient { BaseAddress = new Uri(erpApiUrl) };

        var product = new ErpProduct(
            Id: sku,
            Title: title,
            Description: description,
            Price: price,
            Category: category,
            Brand: brand
        );

        var response = await erpClient.PostAsJsonAsync("/products", product);
        Assert.True(response.IsSuccessStatusCode, $"Failed to setup product in ERP: {response.StatusCode}");
    }

    private async Task<string> SetupProductInErpAndGetSku(
        string skuPrefix,
        string title,
        decimal price,
        string description = "Test product description",
        string category = "Test Category",
        string brand = "Test Brand")
    {
        var uniqueSku = $"{skuPrefix}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        await SetupProductInErp(uniqueSku, title, price, description, category, brand);
        return uniqueSku;
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
