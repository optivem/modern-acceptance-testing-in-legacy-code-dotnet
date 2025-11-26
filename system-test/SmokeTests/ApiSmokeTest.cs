namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ApiSmokeTest
{
    [Fact]
    public async Task Service_ShouldRespond()
    {
        // Arrange
        var httpClient = new HttpClient();
        var baseUrl = TestConfiguration.ShopUiBaseUrl;

        // Act
        var response = await httpClient.GetAsync(baseUrl);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello World!", content);
    }
}
