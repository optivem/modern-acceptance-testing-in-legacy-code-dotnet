using Microsoft.Playwright;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;

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
        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
        { 
            Headless = true 
        }).GetAwaiter().GetResult();
        _context = _browser.NewContextAsync().GetAwaiter().GetResult();
        _page = _browser.NewPageAsync().GetAwaiter().GetResult();
        _pageClient = new TestPageClient(_page, baseUrl);
        _homePage = new HomePage(_pageClient);
    }

    public HomePage OpenHomePage()
    {
        _response = _page.GotoAsync(_baseUrl).GetAwaiter().GetResult();
        return _homePage;
    }

    public bool IsStatusOk()
    {
        return _response?.Status == 200;
    }

    public void AssertPageLoaded()
    {
        if (!IsStatusOk())
        {
            throw new Exception($"Page failed to load with status {_response?.Status}");
        }

        var contentType = _response?.Headers.GetValueOrDefault(ContentType);
        if (contentType == null || !contentType.Contains(TextHtml))
        {
            throw new Exception($"Expected content type {TextHtml}, but got {contentType}");
        }

        var body = _page.ContentAsync().GetAwaiter().GetResult();
        if (!body.Contains(HtmlOpeningTag) || !body.Contains(HtmlClosingTag))
        {
            throw new Exception("Response body does not contain valid HTML");
        }
    }

    public void Dispose()
    {
        _context.CloseAsync().GetAwaiter().GetResult();
        _browser.CloseAsync().GetAwaiter().GetResult();
        _playwright.Dispose();
    }
}
