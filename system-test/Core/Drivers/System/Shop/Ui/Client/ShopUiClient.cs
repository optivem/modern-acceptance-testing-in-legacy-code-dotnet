using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui.Client.Pages;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Shop.Ui.Client;

public class ShopUiClient : IDisposable
{
    private const string ContentType = "content-type";
    private const string TextHtml = "text/html";
    private const string HtmlOpeningTag = "<html";
    private const string HtmlClosingTag = "</html>";

    private readonly string _baseUrl;
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IBrowserContext _context;
    private readonly IPage _page;
    private readonly TestPageClient _pageClient;
    private readonly HomePage _homePage;

    private IResponse? _response;

    public ShopUiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _playwright = Playwright.CreateAsync().Result;
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }).Result;
        _context = _browser.NewContextAsync().Result;
        _page = _browser.NewPageAsync().Result;
        _pageClient = new TestPageClient(_page, baseUrl);
        _homePage = new HomePage(_pageClient);
    }

    public HomePage OpenHomePage()
    {
        _response = _page.GotoAsync(_baseUrl).Result;
        return _homePage;
    }

    public bool IsStatusOk()
    {
        return _response?.Status == 200;
    }

    public void AssertPageLoaded()
    {
        if (_response?.Status != 200)
        {
            throw new InvalidOperationException($"Expected status 200 but got {_response?.Status}");
        }

        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;
        if (contentType == null || !contentType.Contains(TextHtml))
        {
            throw new InvalidOperationException($"Content-Type should be text/html, but was: {contentType}");
        }

        var pageContent = _page.ContentAsync().Result;
        if (!pageContent.Contains(HtmlOpeningTag))
        {
            throw new InvalidOperationException("Response should contain HTML opening tag");
        }
        if (!pageContent.Contains(HtmlClosingTag))
        {
            throw new InvalidOperationException("Response should contain HTML closing tag");
        }
    }

    public void Dispose()
    {
        _page?.CloseAsync().Wait();
        _context?.CloseAsync().Wait();
        _browser?.CloseAsync().Wait();
        _playwright?.Dispose();
    }
}
