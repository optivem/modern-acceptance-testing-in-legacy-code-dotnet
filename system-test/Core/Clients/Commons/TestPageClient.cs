using Microsoft.Playwright;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

public class TestPageClient
{
    private const int DefaultTimeoutMilliseconds = 10000;

    private readonly IPage _page;
    private readonly string _baseUrl;
    private readonly int _timeoutMilliseconds;

    public TestPageClient(IPage page, string baseUrl, int timeoutMilliseconds)
    {
        _page = page;
        _baseUrl = baseUrl;
        _timeoutMilliseconds = timeoutMilliseconds;
    }

    public TestPageClient(IPage page, string baseUrl)
        : this(page, baseUrl, DefaultTimeoutMilliseconds)
    {
    }

    public string BaseUrl => _baseUrl;
    public IPage Page => _page;

    public async Task FillAsync(string selector, string text)
    {
        var input = _page.Locator(selector);
        await WaitAsync(input);
        await input.FillAsync(text);
    }

    public async Task ClickAsync(string selector)
    {
        var button = _page.Locator(selector);
        await WaitAsync(button);
        await button.ClickAsync();
    }

    public async Task<string?> ReadTextContentAsync(string selector)
    {
        var locator = _page.Locator(selector);
        await WaitAsync(locator);
        return await locator.TextContentAsync();
    }

    public async Task<string> ReadInputValueAsync(string selector)
    {
        var locator = _page.Locator(selector);
        await WaitAsync(locator);
        return await locator.InputValueAsync();
    }

    public async Task<bool> IsHiddenAsync(string selector)
    {
        var locator = _page.Locator(selector);
        return await locator.CountAsync() == 0;
    }

    public async Task WaitForHiddenAsync(string selector)
    {
        var locator = _page.Locator(selector);
        await locator.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Hidden,
            Timeout = _timeoutMilliseconds
        });
    }

    private async Task WaitAsync(ILocator locator)
    {
        await locator.WaitForAsync(new LocatorWaitForOptions
        {
            Timeout = _timeoutMilliseconds
        });
    }
}
