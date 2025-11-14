using Microsoft.Playwright;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public class UiE2eTest : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly TestConfiguration _config;

    public UiE2eTest()
    {
        _config = new TestConfiguration();
    }

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    [Fact]
    public async Task PlaceOrder_WithValidInputs_ShouldDisplaySuccessMessage()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{_config.BaseUrl}/shop.html");

        // Act
        await page.FillAsync("#productId", "WIDGET-UI-001");
        await page.FillAsync("#quantity", "5");
        await page.FillAsync("#country", "US");
        await page.ClickAsync("button[type='submit']");

        // Assert
        var resultDiv = page.Locator("#orderResult");
        await resultDiv.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        
        var resultText = await resultDiv.TextContentAsync();
        Assert.NotNull(resultText);
        Assert.Contains("Order placed successfully", resultText);
        Assert.Contains("Order Number:", resultText);
    }

    [Fact]
    public async Task GetOrder_WithExistingOrder_ShouldDisplayOrderDetails()
    {
        // Arrange - First place an order
        var shopPage = await _browser!.NewPageAsync();
        await shopPage.GotoAsync($"{_config.BaseUrl}/shop.html");
        await shopPage.FillAsync("#productId", "WIDGET-UI-002");
        await shopPage.FillAsync("#quantity", "3");
        await shopPage.FillAsync("#country", "US");
        await shopPage.ClickAsync("button[type='submit']");
        
        var resultDiv = shopPage.Locator("#orderResult");
        await resultDiv.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        var resultText = await resultDiv.TextContentAsync();
        var orderNumber = ExtractOrderNumber(resultText!);

        // Act
        var historyPage = await _browser.NewPageAsync();
        await historyPage.GotoAsync($"{_config.BaseUrl}/order-history.html");
        await historyPage.FillAsync("#orderNumber", orderNumber);
        await historyPage.ClickAsync("button[type='submit']");

        // Assert
        await historyPage.WaitForSelectorAsync("#displayOrderNumber");
        
        var displayedOrderNumber = await historyPage.InputValueAsync("#displayOrderNumber");
        Assert.Equal(orderNumber, displayedOrderNumber);
        
        var displayedSku = await historyPage.InputValueAsync("#displayProductId");
        Assert.Equal("WIDGET-UI-002", displayedSku);
        
        var displayedQuantity = await historyPage.InputValueAsync("#displayQuantity");
        Assert.Equal("3", displayedQuantity);
        
        var displayedCountry = await historyPage.InputValueAsync("#displayCountry");
        Assert.Equal("US", displayedCountry);
        
        var displayedStatus = await historyPage.InputValueAsync("#displayStatus");
        Assert.Equal("PLACED", displayedStatus);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentOrder_ShouldDisplayErrorMessage()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{_config.BaseUrl}/order-history.html");

        // Act
        await page.FillAsync("#orderNumber", "NON-EXISTENT-ORDER");
        await page.ClickAsync("button[type='submit']");

        // Assert
        var orderDetails = page.Locator("#orderDetails");
        await orderDetails.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        
        var detailsText = await orderDetails.TextContentAsync();
        Assert.NotNull(detailsText);
        Assert.Contains("Order not found", detailsText);
    }

    [Fact]
    public async Task CancelOrder_WithExistingPlacedOrder_ShouldDisplayCancelledStatus()
    {
        // Arrange - First place an order
        var shopPage = await _browser!.NewPageAsync();
        await shopPage.GotoAsync($"{_config.BaseUrl}/shop.html");
        await shopPage.FillAsync("#productId", "WIDGET-UI-003");
        await shopPage.FillAsync("#quantity", "2");
        await shopPage.FillAsync("#country", "US");
        await shopPage.ClickAsync("button[type='submit']");
        
        var resultDiv = shopPage.Locator("#orderResult");
        await resultDiv.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        var resultText = await resultDiv.TextContentAsync();
        var orderNumber = ExtractOrderNumber(resultText!);

        // Act
        var historyPage = await _browser.NewPageAsync();
        await historyPage.GotoAsync($"{_config.BaseUrl}/order-history.html");
        await historyPage.FillAsync("#orderNumber", orderNumber);
        await historyPage.ClickAsync("button[type='submit']");
        
        await historyPage.WaitForSelectorAsync("#cancelButton");
        await historyPage.ClickAsync("#cancelButton");
        
        // Wait for the page to refresh after cancellation
        await Task.Delay(1000);

        // Assert
        await historyPage.WaitForSelectorAsync("#displayStatus");
        var displayedStatus = await historyPage.InputValueAsync("#displayStatus");
        Assert.Equal("CANCELLED", displayedStatus);
        
        // Verify cancel button is no longer visible
        var cancelButton = historyPage.Locator("#cancelButton");
        await Assertions.Expect(cancelButton).Not.ToBeVisibleAsync();
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
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{_config.BaseUrl}/shop.html");

        // Act
        await page.FillAsync("#productId", sku);
        await page.FillAsync("#quantity", quantity);
        await page.FillAsync("#country", country);
        await page.ClickAsync("button[type='submit']");

        // Assert
        var resultDiv = page.Locator("#orderResult");
        await resultDiv.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        
        var resultText = await resultDiv.TextContentAsync();
        Assert.NotNull(resultText);
        Assert.Contains(expectedError, resultText);
    }

    [Theory]
    [InlineData("abc", "Quantity must be an integer")]
    [InlineData("1.5", "Quantity must be an integer")]
    public async Task PlaceOrder_WithInvalidQuantityType_ShouldDisplayValidationError(
        string quantity, string expectedError)
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{_config.BaseUrl}/shop.html");

        // Act
        await page.FillAsync("#productId", "WIDGET-UI-007");
        await page.FillAsync("#quantity", quantity);
        await page.FillAsync("#country", "US");
        await page.ClickAsync("button[type='submit']");

        // Assert
        var resultDiv = page.Locator("#orderResult");
        await resultDiv.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        
        var resultText = await resultDiv.TextContentAsync();
        Assert.NotNull(resultText);
        Assert.Contains(expectedError, resultText);
    }

    [Fact]
    public async Task NavigateToShop_FromHomePage_ShouldLoadShopPage()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(_config.BaseUrl);

        // Act
        await page.ClickAsync("a[href='/shop.html']");

        // Assert
        await page.WaitForURLAsync($"{_config.BaseUrl}/shop.html");
        var heading = await page.Locator("h1").TextContentAsync();
        Assert.Equal("Shop", heading);
    }

    [Fact]
    public async Task NavigateToOrderHistory_FromHomePage_ShouldLoadOrderHistoryPage()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(_config.BaseUrl);

        // Act
        await page.ClickAsync("a[href='/order-history.html']");

        // Assert
        await page.WaitForURLAsync($"{_config.BaseUrl}/order-history.html");
        var heading = await page.Locator("h1").TextContentAsync();
        Assert.Equal("Order History", heading);
    }

    private static string ExtractOrderNumber(string text)
    {
        // Extract order number from text like "Order placed successfully! Order Number: ORD-123456"
        var parts = text.Split("Order Number:");
        if (parts.Length < 2)
        {
            throw new InvalidOperationException($"Could not extract order number from: {text}");
        }
        return parts[1].Trim();
    }

    public async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
    }
}
