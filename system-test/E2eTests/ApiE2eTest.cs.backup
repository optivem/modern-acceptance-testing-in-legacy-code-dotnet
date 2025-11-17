using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Optivem.EShop.SystemTest.E2eTests.Helpers;

namespace Optivem.EShop.SystemTest.E2eTests;

public class ApiE2eTest : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestConfiguration _config;
    private readonly ErpApiHelper _erpApiHelper;

    public ApiE2eTest()
    {
        _config = new TestConfiguration();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config.BaseUrl)
        };
        _erpApiHelper = new ErpApiHelper(_config);
    }

    [Fact]
    public async Task PlaceOrder_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-PO-100";
        var unitPrice = 99.99m;
        var quantity = 5;

        var sku = await _erpApiHelper.SetupProductInErp(baseSku, "Test Product", unitPrice);

        var request = new
        {
            sku = sku,
            quantity = quantity.ToString(),
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
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-GO-200";
        var unitPrice = 125.50m;
        var quantity = 2;
        var country = "US";

        var sku = await _erpApiHelper.SetupProductInErp(baseSku, "Test Product", unitPrice);

        var placeRequest = new
        {
            sku = sku,
            quantity = quantity.ToString(),
            country = country
        };
        var placeResponse = await _httpClient.PostAsJsonAsync("/api/orders", placeRequest);
        var placeBody = await placeResponse.Content.ReadFromJsonAsync<JsonElement>();
        var orderNumber = placeBody.GetProperty("orderNumber").GetString();

        // Act
        var getResponse = await _httpClient.GetAsync($"/api/orders/{orderNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        // Assert all fields from GetOrderResponse
        var order = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(orderNumber, order.GetProperty("orderNumber").GetString());
        Assert.Equal(sku, order.GetProperty("sku").GetString());
        Assert.Equal(quantity, order.GetProperty("quantity").GetInt32());
        Assert.Equal(country, order.GetProperty("country").GetString());
        Assert.Equal(unitPrice, order.GetProperty("unitPrice").GetDecimal());

        var expectedOriginalPrice = 251.00m;
        Assert.Equal(expectedOriginalPrice, order.GetProperty("originalPrice").GetDecimal());

        Assert.NotNull(order.GetProperty("status").GetString());
        Assert.Equal("PLACED", order.GetProperty("status").GetString());
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
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-CO-300";
        var sku = await _erpApiHelper.SetupProductInErp(baseSku, "Test Product", 99.99m);

        var placeRequest = new
        {
            sku = sku,
            quantity = "2",
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
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-CC-400";
        var sku = await _erpApiHelper.SetupProductInErp(baseSku, "Test Product", 89.99m);

        var placeRequest = new
        {
            sku = sku,
            quantity = "1",
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
    [InlineData("", 5, "US", "SKU must not be empty", false)]
    [InlineData("AUTO-IV-300", 0, "US", "Quantity must be positive", true)]
    [InlineData("AUTO-NQ-400", -1, "US", "Quantity must be positive", true)]
    [InlineData("AUTO-IV-500", 5, "", "Country must not be empty", true)]
    public async Task PlaceOrder_WithInvalidRequest_ShouldReturnBadRequest(
        string sku, int quantity, string country, string expectedError, bool setupErpProduct)
    {
        // Arrange - Set up product in ERP if needed
        var actualSku = sku;
        if (setupErpProduct && !string.IsNullOrEmpty(sku))
        {
            actualSku = await _erpApiHelper.SetupProductInErp(sku, "Test Product", 99.99m);
        }

        var request = new Dictionary<string, object?>
        {
            ["sku"] = string.IsNullOrEmpty(actualSku) ? null : actualSku,
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
        // Arrange - Set up product in ERP first
        var baseSku = "AUTO-EQ-500";
        var sku = await _erpApiHelper.SetupProductInErp(baseSku, "Test Product", 150.00m);

        var request = new Dictionary<string, object?>
        {
            ["sku"] = sku,
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

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
