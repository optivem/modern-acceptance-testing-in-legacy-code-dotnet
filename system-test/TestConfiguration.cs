using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Optivem.EShop.SystemTest;

public static class TestConfiguration
{
    private static readonly TestSettings Settings;

    static TestConfiguration()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        var yamlContent = File.ReadAllText("Resources/application.yml");
        Settings = deserializer.Deserialize<TestSettings>(yamlContent) ?? new TestSettings();
    }

    public static string GetShopUiBaseUrl() => Settings.Shop.UiBaseUrl;
    public static string GetShopApiBaseUrl() => Settings.Shop.ApiBaseUrl;
    public static string GetErpApiBaseUrl() => Settings.Erp.ApiBaseUrl;
    public static string GetTaxApiBaseUrl() => Settings.Tax.ApiBaseUrl;
}

public class TestSettings
{
    public ShopSettings Shop { get; set; } = new();
    public ErpSettings Erp { get; set; } = new();
    public TaxSettings Tax { get; set; } = new();
}

public class ShopSettings
{
    public string UiBaseUrl { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
}

public class ErpSettings
{
    public string ApiBaseUrl { get; set; } = string.Empty;
}

public class TaxSettings
{
    public string ApiBaseUrl { get; set; } = string.Empty;
}
