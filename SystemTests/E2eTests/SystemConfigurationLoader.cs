using Microsoft.Extensions.Configuration;
using Optivem.EShop.SystemTest.Core.Common;

namespace E2eTests;

public static class SystemConfigurationLoader
{
    private static readonly IConfiguration Configuration;

    static SystemConfigurationLoader()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();
    }

    public static SystemConfiguration Load()
    {
        var shopUiBaseUrl = GetValue("Shop:UiBaseUrl");
        var shopApiBaseUrl = GetValue("Shop:ApiBaseUrl");
        var erpBaseUrl = GetValue("Erp:ApiBaseUrl");
        var taxBaseUrl = GetValue("Tax:ApiBaseUrl");
        var clockBaseUrl = GetValue("Clock:ApiBaseUrl");

        return new SystemConfiguration(shopUiBaseUrl, shopApiBaseUrl, erpBaseUrl, taxBaseUrl, clockBaseUrl);
    }

    private static string GetValue(string key)
    {
        var value = Configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Configuration value for '{key}' is missing or empty.");
        }

        return value;
    }
}
