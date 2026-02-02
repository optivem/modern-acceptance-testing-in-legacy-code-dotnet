using System.Text.Json;
using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V1;

public abstract class BaseRawTest : BaseConfigurableTest
{
    protected HttpClient? ErpHttpClient;
    protected HttpClient? TaxHttpClient;
    protected HttpClient? ShopHttpClient;
    protected IPlaywright? Playwright;
    protected IBrowser? Browser;
    protected IBrowserContext? BrowserContext;
    protected IPage? Page;
    protected SystemConfiguration? Configuration;
    protected JsonSerializerOptions? ObjectMapper;

    protected void SetUpExternalHttpClients()
    {
        Configuration = LoadConfiguration();
        ErpHttpClient = new HttpClient();
        TaxHttpClient = new HttpClient();
        ObjectMapper = CreateObjectMapper();
    }

    protected void SetUpShopHttpClient()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }
        ShopHttpClient = new HttpClient();
        if (ObjectMapper == null)
        {
            ObjectMapper = CreateObjectMapper();
        }
    }

    protected void SetUpShopBrowser()
    {
        if (Configuration == null)
        {
            Configuration = LoadConfiguration();
        }

        Playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true,
            SlowMo = 100
        };

        Browser = Playwright.Chromium.LaunchAsync(launchOptions).Result;

        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            StorageStatePath = null
        };

        BrowserContext = Browser.NewContextAsync(contextOptions).Result;
        Page = BrowserContext.NewPageAsync().Result;
    }

    protected string GetErpBaseUrl() => Configuration!.ErpBaseUrl;
    protected string GetTaxBaseUrl() => Configuration!.TaxBaseUrl;
    protected string GetShopApiBaseUrl() => Configuration!.ShopApiBaseUrl;
    protected string GetShopUiBaseUrl() => Configuration!.ShopUiBaseUrl;

    private JsonSerializerOptions CreateObjectMapper()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected virtual void TearDown()
    {
        Page?.CloseAsync().Wait();
        BrowserContext?.CloseAsync().Wait();
        Browser?.CloseAsync().Wait();
        Playwright?.Dispose();
        ErpHttpClient?.Dispose();
        TaxHttpClient?.Dispose();
        ShopHttpClient?.Dispose();
    }
}
