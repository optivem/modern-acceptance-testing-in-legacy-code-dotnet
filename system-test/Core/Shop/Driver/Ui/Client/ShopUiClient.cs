using Microsoft.Playwright;
using Optivem.Http;
using Shouldly;
using System.Net;
using PlaywrightGateway = Optivem.Playwright.PageGateway;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client;

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
    private readonly HomePage _homePage;

    private IResponse? _response;

    public ShopUiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }).Result;
        _context = _browser.NewContextAsync().Result;
        _page = _browser.NewPageAsync().Result;
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
        if (_response == null || _response.Status != (int)HttpStatusCode.OK)
        {
            return false;
        }

        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;
        if (contentType == null || !contentType.Equals(TextHtml))
        {
            return false;
        }

        var pageContent = _page.ContentAsync().Result;
        return pageContent != null && pageContent.Contains(HtmlOpeningTag) && pageContent.Contains(HtmlClosingTag);
    }

    public void AssertPageLoaded()
    {
        _response.ShouldNotBeNull();
        _response.Status.ShouldBe((int)HttpStatusCode.OK);

        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;
        contentType.ShouldNotBeNull();
        contentType.Equals(TextHtml);

        var pageContent = _page.ContentAsync().Result;
        pageContent.ShouldNotBeNull();
        pageContent.ShouldContain(HtmlOpeningTag);
        pageContent.ShouldContain(HtmlClosingTag);
    }

    public void Dispose()
    {
        _page?.CloseAsync().Wait();
        _context?.CloseAsync().Wait();
        _browser?.CloseAsync().Wait();
        _playwright?.Dispose();
    }
}
