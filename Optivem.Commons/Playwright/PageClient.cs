using System.Globalization;
using Microsoft.Playwright;

namespace Optivem.Commons.Playwright;

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

    public string GetBaseUrl() => _baseUrl;

    public IPage GetPage() => _page;
    
    public string? GetPageContent()
    {
        return _page.ContentAsync().Result;
    }

    public void Fill(string selector, string? text)
    {
        var input = _page.Locator(selector);
        Wait(input);
        var safeText = text ?? string.Empty;
        input.FillAsync(safeText).Wait();
    }

    public void Click(string selector)
    {
        var button = _page.Locator(selector);
        Wait(button);
        button.ClickAsync().Wait();
    }

    public string ReadTextContent(string selector)
    {
        var locator = _page.Locator(selector);
        Wait(locator);
        return locator.TextContentAsync().Result ?? string.Empty;
    }

    public List<string> ReadAllTextContents(string selector)
    {
        var locator = _page.Locator(selector);
        Wait(locator);
        var count = locator.CountAsync().Result;
        var texts = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var text = locator.Nth(i).TextContentAsync().Result ?? string.Empty;
            texts.Add(text);
        }
        return texts;
    }

    public bool Exists(string selector)
    {
        var locator = _page.Locator(selector);
        try
        {
            Wait(locator);
            return locator.CountAsync().Result > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool IsVisible(string selector)
    {
        try
        {
            var locator = _page.Locator(selector);
            // Wait for element to appear or timeout - matches Java behavior
            var waitForOptions = GetWaitForOptions();
            waitForOptions.State = WaitForSelectorState.Visible;
            waitForOptions.Timeout = _timeoutMilliseconds;
            
            locator.WaitForAsync(waitForOptions).Wait();
            return locator.CountAsync().Result > 0;
        }
        catch
        {
            return false;
        }
    }

    public string ReadInputValue(string selector)
    {
        var locator = _page.Locator(selector);
        Wait(locator);
        return locator.InputValueAsync().Result;
    }

    public int ReadInputIntegerValue(string selector)
    {
        var inputValue = ReadInputValue(selector);
        return int.Parse(inputValue, CultureInfo.InvariantCulture);
    }

    public decimal ReadInputCurrencyDecimalValue(string selector)
    {
        var inputValue = ReadInputValue(selector);
        inputValue = inputValue.Replace("$", "");
        return decimal.Parse(inputValue, CultureInfo.InvariantCulture);
    }

    public decimal ReadInputPercentageDecimalValue(string selector)
    {
        var inputValue = ReadInputValue(selector);
        inputValue = inputValue.Replace("%", "");
        return decimal.Parse(inputValue, CultureInfo.InvariantCulture);
    }

    public bool IsHidden(string selector)
    {
        var locator = _page.Locator(selector);
        return locator.CountAsync().Result == 0;
    }

    public void WaitForHidden(string selector)
    {
        var waitForOptions = GetWaitForOptions();
        waitForOptions.State = WaitForSelectorState.Hidden;
        waitForOptions.Timeout = _timeoutMilliseconds;

        var locator = _page.Locator(selector);
        locator.WaitForAsync(waitForOptions).Wait();
    }

    public void WaitForVisible(string selector)
    {
        var waitForOptions = GetWaitForOptions();
        waitForOptions.State = WaitForSelectorState.Visible;
        waitForOptions.Timeout = _timeoutMilliseconds;

        var locator = _page.Locator(selector);
        locator.WaitForAsync(waitForOptions).Wait();
    }

    private void Wait(ILocator locator)
    {
        var waitForOptions = GetWaitForOptions();
        locator.WaitForAsync(waitForOptions).Wait();
    }

    private LocatorWaitForOptions GetWaitForOptions()
    {
        return new LocatorWaitForOptions { Timeout = _timeoutMilliseconds };
    }
}
