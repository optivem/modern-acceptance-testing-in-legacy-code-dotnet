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

    public void Fill(string selector, string? text)
    {
        var locator = GetLocator(selector);
        var processedValue = text ?? string.Empty;
        locator.FillAsync(processedValue).Wait();
    }

    public void Click(string selector)
    {
        var locator = GetLocator(selector);
        locator.ClickAsync().Wait();
    }

    public string ReadTextContent(string selector)
    {
        var locator = GetLocator(selector);
        return locator.TextContentAsync().Result ?? string.Empty;
    }

    public List<string> ReadAllTextContents(string selector)
    {
        var locator = _page.Locator(selector);
        // Wait for at least one element to be visible
        // AllTextContentsAsync() doesn't trigger strict mode - it's designed for multiple elements
        locator.First.WaitForAsync(GetDefaultWaitForOptions()).Wait();
        return locator.AllTextContentsAsync().Result.ToList();
    }

    public bool IsVisible(string selector)
    {
        try
        {
            var locator = GetLocator(selector);
            return locator.CountAsync().Result > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool IsHidden(string selector)
    {
        var locator = _page.Locator(selector);
        return locator.CountAsync().Result == 0;
    }

    private ILocator GetLocator(string selector, LocatorWaitForOptions waitForOptions)
    {
        var locator = _page.Locator(selector);
        locator.WaitForAsync(waitForOptions).Wait();

        if (locator.CountAsync().Result == 0)
        {
            throw new Exception($"No elements found for selector: {selector}");
        }

        return locator;
    }

    private ILocator GetLocator(string selector)
    {
        var waitForOptions = GetDefaultWaitForOptions();
        return GetLocator(selector, waitForOptions);
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
