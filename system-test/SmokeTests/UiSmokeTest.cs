using Microsoft.Playwright;

namespace Optivem.EShop.SystemTest.SmokeTests;

public class UiSmokeTest : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly TestConfiguration _config;

    public UiSmokeTest()
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
    public async Task HomePage_ShouldLoadSuccessfully()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        
        // Act
        await page.GotoAsync(_config.BaseUrl);
        
        // Assert
        var title = await page.TitleAsync();
        Assert.Equal("Optivem eShop (.NET)", title);
        
        var heading = await page.Locator("h1").TextContentAsync();
        Assert.Equal("Optivem eShop (.NET)", heading);
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
