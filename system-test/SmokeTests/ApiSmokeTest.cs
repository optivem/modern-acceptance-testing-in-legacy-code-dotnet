using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ApiSmokeTest : IDisposable
{
    private readonly ShopApiClient _shopApiClient;

    public ApiSmokeTest()
    {
        _shopApiClient = ClientFactory.CreateShopApiClient();
    }

    public void Dispose()
    {
        ClientCloser.Close(_shopApiClient);
    }

    //[Fact]
    //public void Echo_ShouldReturnOk()
    //{
    //    // Act
    //    var result = _shopApiClient.Echo().Echo();

    //    // Assert
    //    Assert.True(result.Success, $"Expected successful response but got errors: {string.Join(", ", result.IsFailure ? result.Errors : new List<string>())}");
    //}
}
