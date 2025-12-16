using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Ui.Client.Pages;
using Optivem.Http;
using Optivem.Lang;
using Shouldly;
using System.Net;
using System.Net.NetworkInformation;
using PlaywrightGateway = Optivem.Playwright.PageClient;

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

    public Result<VoidValue, Error> CheckStatusOk()
    {
        if(_response?.Status == ((int)HttpStatusCode.OK))
        {
            return Results.Success();
        }

        return Results.Failure<VoidValue>("Could not open shop UI at url " + _baseUrl + " due to status code: " + _response?.Status);
    }

    public Result<VoidValue, Error> CheckPageLoaded()
    {
        var contentType = _response.Headers.ContainsKey(ContentType) ? _response.Headers[ContentType] : null;

        if(contentType == null)
        {
            return Results.Failure<VoidValue>(ContentType + " is missing or empty");
        }

        if(!contentType.Equals(TextHtml))
        {
            return Results.Failure<VoidValue>(ContentType + " is not " + TextHtml + " but instead " + contentType);
        }

        var pageContent = _page.ContentAsync().Result;

        if(pageContent == null)
        {
            return Results.Failure<VoidValue>("Page content is missing");
        }

        if(!pageContent.Contains(HtmlOpeningTag))
        {
            return Results.Failure<VoidValue>("Page content " + pageContent + " does not contain " + HtmlOpeningTag);
        }

        if (!pageContent.Contains(HtmlClosingTag))
        {
            return Results.Failure<VoidValue>("Page content " + pageContent + " does not contain " + HtmlClosingTag);
        }

        return Results.Success();
    }

    public void Dispose()
    {
        _page?.CloseAsync().Wait();
        _context?.CloseAsync().Wait();
        _browser?.CloseAsync().Wait();
        _playwright?.Dispose();
    }
}
