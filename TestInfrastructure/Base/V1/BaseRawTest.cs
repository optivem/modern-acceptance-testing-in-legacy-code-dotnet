using System.Text.Json;
using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V1;

/// <summary>
/// V1: Raw test infrastructure with manual HTTP clients and Playwright setup.
/// Tests directly interact with HttpClient and Playwright APIs.
/// </summary>
public abstract class BaseRawTest : BaseConfigurableTest, IAsyncLifetime
{
    protected HttpClient? ErpHttpClient { get; private set; }
    protected HttpClient? TaxHttpClient { get; private set; }
    protected HttpClient? ShopHttpClient { get; private set; }
    protected IPlaywright? Playwright { get; private set; }
    protected IBrowser? Browser { get; private set; }
    protected IBrowserContext? BrowserContext { get; private set; }
    protected IPage? Page { get; private set; }
    protected SystemConfiguration? Configuration { get; private set; }
    protected JsonSerializerOptions? JsonOptions { get; private set; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    protected void SetUpExternalHttpClients()
    {
        Configuration = LoadConfiguration();
        ErpHttpClient = new HttpClient { BaseAddress = new Uri(Configuration.ErpBaseUrl) };
        TaxHttpClient = new HttpClient { BaseAddress = new Uri(Configuration.TaxBaseUrl) };
        JsonOptions = CreateJsonOptions();
    }

    protected void SetUpShopHttpClient()
    {
        Configuration ??= LoadConfiguration();
        ShopHttpClient = new HttpClient { BaseAddress = new Uri(Configuration.ShopApiBaseUrl) };
        JsonOptions ??= CreateJsonOptions();
    }

    protected async Task SetUpShopBrowserAsync()
    {
        Configuration ??= LoadConfiguration();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new()
        {
            Headless = true,
            SlowMo = 100
        });

        BrowserContext = await Browser.NewContextAsync(new()
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        Page = await BrowserContext.NewPageAsync();
    }

    protected string GetErpBaseUrl() => Configuration!.ErpBaseUrl;
    protected string GetTaxBaseUrl() => Configuration!.TaxBaseUrl;
    protected string GetShopApiBaseUrl() => Configuration!.ShopApiBaseUrl;
    protected string GetShopUiBaseUrl() => Configuration!.ShopUiBaseUrl;

    private JsonSerializerOptions CreateJsonOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public virtual async Task DisposeAsync()
    {
        Page?.CloseAsync().Wait();
        await (BrowserContext?.CloseAsync() ?? Task.CompletedTask);
        await (Browser?.CloseAsync() ?? Task.CompletedTask);
        Playwright?.Dispose();
        ErpHttpClient?.Dispose();
        TaxHttpClient?.Dispose();
        ShopHttpClient?.Dispose();
    }
}
