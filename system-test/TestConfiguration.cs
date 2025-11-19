using Microsoft.Extensions.Configuration;

namespace Optivem.AtddAccelerator.EShop.SystemTest;

public class TestConfiguration
{
    private static readonly IConfiguration _configuration;

    static TestConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static int WaitSeconds => int.Parse(_configuration["WaitSeconds"] ?? "10");

    // EShop configuration
    public static string ShopUiBaseUrl => _configuration["Test:EShop:Ui:BaseUrl"] ?? "http://localhost:8081";
    
    public static string ShopApiBaseUrl => _configuration["Test:EShop:Api:BaseUrl"] ?? "http://localhost:8081/api";
    
    // ERP configuration
    public static string ErpApiBaseUrl => _configuration["Test:Erp:Api:BaseUrl"] ?? "http://localhost:3100";

    // Tax configuration
    public static string TaxApiBaseUrl => _configuration["Test:Tax:Api:BaseUrl"] ?? "http://localhost:3101";
}
