using Microsoft.Extensions.Configuration;

namespace Optivem.EShop.SystemTest;

public static class TestConfiguration
{
    private static readonly IConfiguration Configuration;

    static TestConfiguration()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();
    }

    public static string GetShopUiBaseUrl() => Configuration["Shop:UiBaseUrl"] ?? string.Empty;
    public static string GetShopApiBaseUrl() => Configuration["Shop:ApiBaseUrl"] ?? string.Empty;
    public static string GetErpApiBaseUrl() => Configuration["Erp:ApiBaseUrl"] ?? string.Empty;
    public static string GetTaxApiBaseUrl() => Configuration["Tax:ApiBaseUrl"] ?? string.Empty;
}


