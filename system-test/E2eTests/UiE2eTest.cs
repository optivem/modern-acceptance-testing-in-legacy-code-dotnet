using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;

namespace Optivem.EShop.SystemTest.E2eTests;

public class UiE2eTest : IAsyncLifetime
{
    private ShopUiClient? _shopUiClient;
    private ErpApiClient? _erpApiClient;

    public Task InitializeAsync()
    {
        _shopUiClient = ClientFactory.CreateShopUiClient();
        _erpApiClient = ClientFactory.CreateErpApiClient();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task PlaceOrder_WithValidInputs_ShouldDisplaySuccessMessage()
    {
        // Arrange
        var baseSku = "AUTO-UI-001";
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, 99.99m);

        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();

        // Act
        await newOrderPage.InputProductIdAsync(sku);
        await newOrderPage.InputQuantityAsync("5");
        await newOrderPage.InputCountryAsync("US");
        await newOrderPage.ClickPlaceOrderAsync();

        // Assert
        await newOrderPage.AssertOrderPlacedSuccessfullyAsync();
        var orderNumber = await newOrderPage.ExtractOrderNumberAsync();
        Assert.NotNull(orderNumber);
        Assert.True(orderNumber.StartsWith("ORD-"));
    }

    [Fact]
    public async Task PlaceOrder_ShouldCalculateOriginalOrderPrice()
    {
        // Arrange
        var baseSku = "AUTO-UI-001B";
        var unitPrice = 109.95m;
        var quantity = 5;
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, unitPrice);

        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();

        // Act
        await newOrderPage.InputProductIdAsync(sku);
        await newOrderPage.InputQuantityAsync(quantity.ToString());
        await newOrderPage.InputCountryAsync("US");
        await newOrderPage.ClickPlaceOrderAsync();

        // Assert
        var originalPrice = await newOrderPage.ExtractOriginalPriceAsync();
        var expectedOriginalPrice = 549.75m;
        Assert.Equal(expectedOriginalPrice, originalPrice, 2);
    }

    [Fact]
    public async Task GetOrder_WithExistingOrder_ShouldDisplayOrderDetails()
    {
        // Arrange
        var baseSku = "AUTO-UI-002";
        var unitPrice = 499.99m;
        var quantity = 3;
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, unitPrice);

        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();
        await newOrderPage.InputProductIdAsync(sku);
        await newOrderPage.InputQuantityAsync(quantity.ToString());
        await newOrderPage.InputCountryAsync("US");
        await newOrderPage.ClickPlaceOrderAsync();
        
        var orderNumber = await newOrderPage.ExtractOrderNumberAsync();

        // Act - Navigate back to home page first, then to order history
        homePage = await _shopUiClient!.OpenHomePageAsync();
        var orderHistoryPage = await homePage.ClickOrderHistoryAsync();
        await orderHistoryPage.InputOrderNumberAsync(orderNumber);
        await orderHistoryPage.ClickViewOrderAsync();

        // Assert
        await orderHistoryPage.AssertOrderDetailsDisplayedAsync();
        
        var displayedOrderNumber = await orderHistoryPage.GetDisplayedOrderNumberAsync();
        Assert.Equal(orderNumber, displayedOrderNumber);
        
        var displayedSku = await orderHistoryPage.GetDisplayedSkuAsync();
        Assert.Equal(sku, displayedSku);
        
        var displayedQuantity = await orderHistoryPage.GetDisplayedQuantityAsync();
        Assert.Equal("3", displayedQuantity);
        
        var displayedCountry = await orderHistoryPage.GetDisplayedCountryAsync();
        Assert.Equal("US", displayedCountry);
        
        var displayedUnitPrice = await orderHistoryPage.GetDisplayedUnitPriceAsync();
        Assert.Equal("$499.99", displayedUnitPrice);
        
        var displayedOriginalPrice = await orderHistoryPage.GetDisplayedOriginalPriceAsync();
        Assert.Equal("$1499.97", displayedOriginalPrice);
        
        var displayedStatus = await orderHistoryPage.GetDisplayedStatusAsync();
        Assert.Equal("PLACED", displayedStatus);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentOrder_ShouldDisplayErrorMessage()
    {
        // Arrange
        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var orderHistoryPage = await homePage.ClickOrderHistoryAsync();

        // Act
        await orderHistoryPage.InputOrderNumberAsync("NON-EXISTENT-ORDER");
        await orderHistoryPage.ClickViewOrderAsync();

        // Assert
        await orderHistoryPage.AssertOrderNotFoundAsync();
    }

    [Fact]
    public async Task CancelOrder_WithExistingPlacedOrder_ShouldDisplayCancelledStatus()
    {
        // Arrange
        var baseSku = "AUTO-UI-003";
        var sku = await _erpApiClient!.Products().CreateProductAsync(baseSku, 79.99m);

        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();
        await newOrderPage.InputProductIdAsync(sku);
        await newOrderPage.InputQuantityAsync("2");
        await newOrderPage.InputCountryAsync("US");
        await newOrderPage.ClickPlaceOrderAsync();
        
        var orderNumber = await newOrderPage.ExtractOrderNumberAsync();

        // Act - Navigate back to home page first, then to order history
        homePage = await _shopUiClient!.OpenHomePageAsync();
        var orderHistoryPage = await homePage.ClickOrderHistoryAsync();
        await orderHistoryPage.InputOrderNumberAsync(orderNumber);
        await orderHistoryPage.ClickViewOrderAsync();
        await orderHistoryPage.ClickCancelOrderAsync();

        // Assert
        await orderHistoryPage.AssertOrderCancelledAsync();
        await orderHistoryPage.AssertCancelButtonNotVisibleAsync();
    }

    [Theory]
    [InlineData("", "5", "US", "SKU must not be empty")]
    [InlineData("WIDGET-UI-004", "0", "US", "Quantity must be positive")]
    [InlineData("WIDGET-UI-005", "-1", "US", "Quantity must be positive")]
    [InlineData("WIDGET-UI-006", "5", "", "Country must not be empty")]
    public async Task PlaceOrder_WithInvalidInputs_ShouldDisplayValidationError(
        string sku, string quantity, string country, string expectedError)
    {
        // Arrange
        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();

        // Act
        await newOrderPage.InputProductIdAsync(sku);
        await newOrderPage.InputQuantityAsync(quantity);
        await newOrderPage.InputCountryAsync(country);
        await newOrderPage.ClickPlaceOrderAsync();

        // Assert
        await newOrderPage.AssertValidationErrorAsync(expectedError);
    }

    [Theory]
    [InlineData("abc", "Quantity must be an integer")]
    [InlineData("1.5", "Quantity must be an integer")]
    public async Task PlaceOrder_WithInvalidQuantityType_ShouldDisplayValidationError(
        string quantity, string expectedError)
    {
        // Arrange
        var homePage = await _shopUiClient!.OpenHomePageAsync();
        var newOrderPage = await homePage.ClickNewOrderAsync();

        // Act
        await newOrderPage.InputProductIdAsync("WIDGET-UI-007");
        await newOrderPage.InputQuantityAsync(quantity);
        await newOrderPage.InputCountryAsync("US");
        await newOrderPage.ClickPlaceOrderAsync();

        // Assert
        await newOrderPage.AssertValidationErrorAsync(expectedError);
    }

    [Fact]
    public async Task NavigateToShop_FromHomePage_ShouldLoadShopPage()
    {
        // Arrange
        var homePage = await _shopUiClient!.OpenHomePageAsync();

        // Act
        var newOrderPage = await homePage.ClickNewOrderAsync();

        // Assert
        await newOrderPage.AssertNewOrderPageLoadedAsync();
    }

    [Fact]
    public async Task NavigateToOrderHistory_FromHomePage_ShouldLoadOrderHistoryPage()
    {
        // Arrange
        var homePage = await _shopUiClient!.OpenHomePageAsync();

        // Act
        var orderHistoryPage = await homePage.ClickOrderHistoryAsync();

        // Assert
        await orderHistoryPage.AssertOrderHistoryPageLoadedAsync();
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_shopUiClient);
        await ClientCloser.CloseAsync(_erpApiClient);
    }
}
