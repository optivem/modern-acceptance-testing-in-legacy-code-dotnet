using System.Text.Json;
using Microsoft.Playwright;
using Optivem.EShop.SystemTest.Configuration;
using Optivem.EShop.SystemTest.Core;
using Xunit;

namespace Optivem.EShop.SystemTest.Base.V1;

public abstract class BaseRawTest : BaseConfigurableTest, IAsyncLifetime
{
    protected readonly SystemConfiguration _configuration;
    
    protected IPlaywright? shopUiPlaywright;
    protected IBrowser? shopUiBrowser;
    protected IBrowserContext? shopUiBrowserContext;
    protected IPage? shopUiPage;
    protected HttpClient? _shopApiHttpClient;
    
    protected HttpClient? _erpHttpClient;
    protected HttpClient? _taxHttpClient;

    protected JsonSerializerOptions? _httpObjectMapper;

    protected BaseRawTest() 
    {
        _configuration = LoadConfiguration();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    protected async Task SetUpShopBrowserAsync()
    {
        shopUiPlaywright = await Playwright.CreateAsync();

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true,
            SlowMo = 100
        };

        shopUiBrowser = await shopUiPlaywright.Chromium.LaunchAsync(launchOptions);

        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            StorageStatePath = null
        };

        shopUiBrowserContext = await shopUiBrowser.NewContextAsync(contextOptions);
        shopUiPage = await shopUiBrowserContext.NewPageAsync();
    }

    protected void SetUpShopHttpClient()
    {
        _shopApiHttpClient = new HttpClient();
        if (_httpObjectMapper == null)
        {
            _httpObjectMapper = CreateObjectMapper();
        }
    }

    protected void SetUpExternalHttpClients()
    {
        _erpHttpClient = new HttpClient();
        _taxHttpClient = new HttpClient();
        _httpObjectMapper = CreateObjectMapper();
    }

    private JsonSerializerOptions CreateObjectMapper()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public virtual async Task DisposeAsync()
    {
        if (shopUiPage != null)
            await shopUiPage.CloseAsync();
        if (shopUiBrowserContext != null)
            await shopUiBrowserContext.CloseAsync();
        if (shopUiBrowser != null)
            await shopUiBrowser.CloseAsync();
        shopUiPlaywright?.Dispose();

        _shopApiHttpClient?.Dispose();
        _erpHttpClient?.Dispose();
        _taxHttpClient?.Dispose();
    }
}
