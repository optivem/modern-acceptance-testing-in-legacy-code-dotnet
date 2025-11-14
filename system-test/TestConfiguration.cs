using Microsoft.Extensions.Configuration;

namespace Optivem.AtddAccelerator.EShop.SystemTest;

public class TestConfiguration
{
    private readonly IConfiguration _configuration;

    public TestConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public string BaseUrl => _configuration["BaseUrl"] ?? "http://localhost:8080";
    
    public int WaitSeconds => int.Parse(_configuration["WaitSeconds"] ?? "10");
}
