using System.Net;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ApiSmokeTest : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TestConfiguration _config;

    public ApiSmokeTest()
    {
        _config = new TestConfiguration();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config.BaseUrl)
        };
    }

    [Fact]
    public async Task Echo_ShouldReturnOk()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/echo");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
