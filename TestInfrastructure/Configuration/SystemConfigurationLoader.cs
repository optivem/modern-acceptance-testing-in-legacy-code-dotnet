using Microsoft.Extensions.Configuration;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Common;

namespace Optivem.EShop.SystemTest.Configuration;

public static class SystemConfigurationLoader
{
    public static SystemConfiguration Load(Environment environment, ExternalSystemMode externalSystemMode)
    {
        var configFile = GetConfigFileName(environment, externalSystemMode);
        var configuration = LoadJsonFile(configFile);

        var shopUiBaseUrl = GetValue(configuration, "Shop:UiBaseUrl");
        var shopApiBaseUrl = GetValue(configuration, "Shop:ApiBaseUrl");
        var erpBaseUrl = GetValue(configuration, "Erp:ApiBaseUrl");
        var taxBaseUrl = GetValue(configuration, "Tax:ApiBaseUrl");
        var clockBaseUrl = GetValue(configuration, "Clock:ApiBaseUrl");

        return new SystemConfiguration(shopUiBaseUrl, shopApiBaseUrl, erpBaseUrl, taxBaseUrl, clockBaseUrl, externalSystemMode);
    }

    private static string GetConfigFileName(Environment environment, ExternalSystemMode externalSystemMode)
    {
        var env = environment.ToString().ToLower();
        var mode = externalSystemMode.ToString().ToLower();
        return $"appsettings.{env}.{mode}.json";
    }

    private static IConfiguration LoadJsonFile(string fileName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(fileName, optional: false, reloadOnChange: false)
            .Build();

        if (configuration == null)
        {
            throw new InvalidOperationException($"Configuration file not found: {fileName}");
        }

        return configuration;
    }

    private static string GetValue(IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Configuration value for '{key}' is missing or empty.");
        }

        return value;
    }
}
