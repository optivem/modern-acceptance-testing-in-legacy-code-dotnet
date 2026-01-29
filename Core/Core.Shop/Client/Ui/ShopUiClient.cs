using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Client.Ui.Pages;
using Commons.Playwright;
using System.Net;
using PlaywrightGateway = Commons.Playwright.PageClient;

namespace Optivem.EShop.SystemTest.Core.Shop.Client.Ui;

public class ShopUiClient : IDisposable
{
    // Default: headless mode (browser not visible)
    // To see browser during debugging, set: HEADED=true or PLAYWRIGHT_HEADED=true
    private static readonly bool IsHeadless = Environment.GetEnvironmentVariable("HEADED") != "true" 
        && Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADED") != "true";

    private const string ContentType = "content-type";
    private const string TextHtml = "text/html";
    private const string HtmlOpeningTag = "<html";
    private const string HtmlClosingTag = "</html>";

    private readonly string _baseUrl;
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IBrowserContext _context;
    private readonly IPage _page;
    private readonly HomePage _homePage;

    private IResponse? _response;

    public ShopUiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = IsHeadless }).Result;
        
        // Create isolated browser context with specific configuration
        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            StorageStatePath = null // Ensure complete isolation between parallel tests
        };
        _context = _browser.NewContextAsync(contextOptions).Result;
        
        // Each test gets its own page
        _page = _context.NewPageAsync().Result;
        var pageClient = new PlaywrightGateway(_page, baseUrl);
        _homePage = new HomePage(pageClient);
    }

    public HomePage OpenHomePage()
    {
        _response = _page.GotoAsync(_baseUrl).Result;
        return _homePage;
    }

    public bool IsStatusOk()
    {
        return _response?.Status == ((int)HttpStatusCode.OK);
    }

    public bool IsPageLoaded()
    {
        if (_response == null || _response.Status != ((int)HttpStatusCode.OK))
        {
            return false;
        }

        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;
        if (contentType == null || !contentType.Equals(TextHtml))
        {
            return false;
        }

        var pageContent = _page.ContentAsync().Result;
        return pageContent != null && 
               pageContent.Contains(HtmlOpeningTag) && 
               pageContent.Contains(HtmlClosingTag);
    }

    public void Dispose()
    {
        _page?.CloseAsync().Wait();
        _context?.CloseAsync().Wait();
        // Keep browser and playwright disposal for C# version since we create our own instance
        _browser?.CloseAsync().Wait();
        _playwright?.Dispose();
    }
}
