using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;
using Optivem.Http;
using Optivem.Results;
using Shouldly;
using System.Net;
using System.Net.NetworkInformation;
using PlaywrightGateway = Optivem.Playwright.PageGateway;

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

    public Result<VoidResult> CheckStatusOk()
    {
        if(_response?.Status == ((int)HttpStatusCode.OK))
        {
            return Result.Success();
        }

        return Result.Failure("Could not open shop UI at url " + _baseUrl + " due to status code: " + _response?.Status);
    }

    public Result<VoidResult> CheckPageLoaded()
    {
        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;

        if(contentType == null)
        {
            return Result.Failure(ContentType + " is missing or empty");
        }

        if(!contentType.Equals(TextHtml))
        {
            return Result.Failure(ContentType + " is not " + TextHtml + " but instead " + contentType);
        }

        var pageContent = _page.ContentAsync().Result;

        if(pageContent == null)
        {
            return Result.Failure("Page content is missing");
        }

        if(!pageContent.Contains(HtmlOpeningTag))
        {
            return Result.Failure("Page content " + pageContent + " does not contain " + HtmlOpeningTag);
        }

        if (!pageContent.Contains(HtmlClosingTag))
        {
            return Result.Failure("Page content " + pageContent + " does not contain " + HtmlClosingTag);
        }

        return Result.Success();
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
