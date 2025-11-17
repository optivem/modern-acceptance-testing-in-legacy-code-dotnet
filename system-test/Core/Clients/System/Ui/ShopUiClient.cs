using Microsoft.Playwright;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui.Pages;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;

public class ShopUiClient : IAsyncDisposable
{
    private const string ContentType = "content-type";
    private const string TextHtml = "text/html";
    private const string HtmlOpeningTag = "<html";
    private const string HtmlClosingTag = "</html>";

    private readonly string _baseUrl;
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IPage _page;
    private readonly TestPageClient _pageClient;
    private readonly HomePage _homePage;

    private IResponse? _response;

    public ShopUiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _playwright = Playwright.CreateAsync().Result;
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        }).Result;
        _page = _browser.NewPageAsync().Result;
        _pageClient = new TestPageClient(_page, baseUrl);
        _homePage = new HomePage(_pageClient);
    }

    public async Task<HomePage> OpenHomePageAsync()
    {
        _response = await _page.GotoAsync(_baseUrl);
        return _homePage;
    }

    public void AssertHomePageLoaded()
    {
        Assert.NotNull(_response);
        Assert.Equal(200, _response.Status);

        var contentType = _response.Headers.GetValueOrDefault(ContentType);
        Assert.True(contentType != null && contentType.Contains(TextHtml),
            $"Content-Type should be text/html, but was: {contentType}");

        // Check HTML structure using Playwright's content method
        var pageContent = _page.ContentAsync().Result;
        Assert.Contains(HtmlOpeningTag, pageContent);
        Assert.Contains(HtmlClosingTag, pageContent);
    }

    public async ValueTask DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        if (_playwright != null)
        {
            _playwright.Dispose();
        }
    }
}
