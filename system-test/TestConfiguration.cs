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

    public static string BaseUrl => _configuration["BaseUrl"] ?? "http://localhost:8081";
    
    public static int WaitSeconds => int.Parse(_configuration["WaitSeconds"] ?? "10");

    // Client layer configuration
    public static string ShopUiBaseUrl => _configuration["ShopUiBaseUrl"] ?? BaseUrl;
    
    public static string ShopApiBaseUrl => _configuration["ShopApiBaseUrl"] ?? $"{BaseUrl}/api";
    
    public static string ErpApiBaseUrl => _configuration["ErpApiBaseUrl"] ?? BaseUrl.Replace(":8081", ":3100");
}
