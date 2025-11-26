using Microsoft.Playwright;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.Commons;

public class TestPageClient
{
    private readonly IPage _page;
    private readonly string _baseUrl;
    private readonly int _timeoutMilliseconds;

    private const int DefaultTimeoutSeconds = 10;
    private const int DefaultTimeoutMilliseconds = DefaultTimeoutSeconds * 1000;

    private TestPageClient(IPage page, string baseUrl, int timeoutMilliseconds)
    {
        _page = page;
        _baseUrl = baseUrl;
        _timeoutMilliseconds = timeoutMilliseconds;
    }

    public TestPageClient(IPage page, string baseUrl) 
        : this(page, baseUrl, DefaultTimeoutMilliseconds)
    {
    }

    public string GetBaseUrl() => _baseUrl;

    public IPage GetPage() => _page;

    public void Fill(string selector, string text)
    {
        var input = _page.Locator(selector);
        Wait(input);
        input.FillAsync(text).GetAwaiter().GetResult();
    }

    public void Click(string selector)
    {
        var button = _page.Locator(selector);
        Wait(button);
        button.ClickAsync().GetAwaiter().GetResult();
    }

    public string ReadTextContent(string selector)
    {
        var locator = _page.Locator(selector);
        Wait(locator);
        return locator.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;
    }

    public bool Exists(string selector)
    {
        var locator = _page.Locator(selector);
        try
        {
            Wait(locator);
            return locator.CountAsync().GetAwaiter().GetResult() > 0;
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
        return locator.InputValueAsync().GetAwaiter().GetResult();
    }

    public decimal ReadInputCurrencyDecimalValue(string selector)
    {
        var inputValue = ReadInputValue(selector);
        inputValue = inputValue.Replace("$", "");
        return decimal.Parse(inputValue);
    }

    public decimal ReadInputPercentageDecimalValue(string selector)
    {
        var inputValue = ReadInputValue(selector);
        inputValue = inputValue.Replace("%", "");
        return decimal.Parse(inputValue);
    }

    public bool IsHidden(string selector)
    {
        var locator = _page.Locator(selector);
        return locator.CountAsync().GetAwaiter().GetResult() == 0;
    }

    public void WaitForHidden(string selector)
    {
        var waitForOptions = GetWaitForOptions();
        waitForOptions.State = WaitForSelectorState.Hidden;
        waitForOptions.Timeout = _timeoutMilliseconds;

        var locator = _page.Locator(selector);
        locator.WaitForAsync(waitForOptions).GetAwaiter().GetResult();
    }

    public void WaitForVisible(string selector)
    {
        var waitForOptions = GetWaitForOptions();
        waitForOptions.State = WaitForSelectorState.Visible;
        waitForOptions.Timeout = _timeoutMilliseconds;

        var locator = _page.Locator(selector);
        locator.WaitForAsync(waitForOptions).GetAwaiter().GetResult();
    }

    private void Wait(ILocator locator)
    {
        try
        {
            var waitForOptions = GetWaitForOptions();
            locator.WaitForAsync(waitForOptions).GetAwaiter().GetResult();
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"Element not found or not visible within {_timeoutMilliseconds}ms", ex);
        }
    }

    private LocatorWaitForOptions GetWaitForOptions()
    {
        return new LocatorWaitForOptions
        {
            Timeout = _timeoutMilliseconds
        };
    }
}
