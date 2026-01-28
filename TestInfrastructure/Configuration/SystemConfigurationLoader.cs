using Microsoft.Extensions.Configuration;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Common;

namespace Optivem.EShop.SystemTest.Configuration;

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

    public static SystemConfiguration Load(ExternalSystemMode externalSystemMode)
    {
        var shopUiBaseUrl = GetValue("Shop:UiBaseUrl");
        var shopApiBaseUrl = GetValue("Shop:ApiBaseUrl");
        var erpBaseUrl = GetValue("Erp:ApiBaseUrl");
        var taxBaseUrl = GetValue("Tax:ApiBaseUrl");
        var clockBaseUrl = GetValue("Clock:ApiBaseUrl");

        return new SystemConfiguration(shopUiBaseUrl, shopApiBaseUrl, erpBaseUrl, taxBaseUrl, clockBaseUrl, externalSystemMode);
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
