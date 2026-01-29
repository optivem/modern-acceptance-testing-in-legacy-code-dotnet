using Microsoft.Playwright;

namespace Commons.Playwright;

public class PageClient
{
    private readonly IPage _page;
    private readonly string _baseUrl;
    private readonly float _timeoutMilliseconds;

    // Increase timeout to match Java's 30 seconds for better stability
    private const int DefaultTimeoutSeconds = 30;
    private const int DefaultTimeoutMilliseconds = DefaultTimeoutSeconds * 1000;

    private PageClient(IPage page, string baseUrl, float timeoutMilliseconds)
    {
        _page = page;
        _baseUrl = baseUrl;
        _timeoutMilliseconds = timeoutMilliseconds;
    }

    public PageClient(IPage page, string baseUrl) 
        : this(page, baseUrl, DefaultTimeoutMilliseconds)
    {
    }

    public async Task FillAsync(string selector, string? text)
    {
        var locator = await GetLocatorAsync(selector);
        var processedValue = text ?? string.Empty;
        await locator.FillAsync(processedValue);
    }

    public async Task ClickAsync(string selector)
    {
        var locator = await GetLocatorAsync(selector);
        await locator.ClickAsync();
    }

    public async Task<string> ReadTextContentAsync(string selector)
    {
        var locator = await GetLocatorAsync(selector);
        return await locator.TextContentAsync() ?? string.Empty;
    }

    public async Task<List<string>> ReadAllTextContentsAsync(string selector)
    {
        var locator = _page.Locator(selector);
        // Wait for at least one element to be visible
        // AllTextContentsAsync() doesn't trigger strict mode - it's designed for multiple elements
        await locator.First.WaitForAsync(GetDefaultWaitForOptions());
        var contents = await locator.AllTextContentsAsync();
        return contents.ToList();
    }

    public async Task<bool> IsVisibleAsync(string selector)
    {
        try
        {
            var locator = await GetLocatorAsync(selector);
            return await locator.CountAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsHiddenAsync(string selector)
    {
        var locator = _page.Locator(selector);
        return await locator.CountAsync() == 0;
    }

    private async Task<ILocator> GetLocatorAsync(string selector, LocatorWaitForOptions waitForOptions)
    {
        var locator = _page.Locator(selector);
        await locator.WaitForAsync(waitForOptions);

        if (await locator.CountAsync() == 0)
        {
            throw new Exception($"No elements found for selector: {selector}");
        }

        return locator;
    }

    private async Task<ILocator> GetLocatorAsync(string selector)
    {
        var waitForOptions = GetDefaultWaitForOptions();
        return await GetLocatorAsync(selector, waitForOptions);
    }

    private LocatorWaitForOptions GetDefaultWaitForOptions()
    {
        return new LocatorWaitForOptions 
        { 
            State = WaitForSelectorState.Visible,
            Timeout = _timeoutMilliseconds 
        };
    }
}
