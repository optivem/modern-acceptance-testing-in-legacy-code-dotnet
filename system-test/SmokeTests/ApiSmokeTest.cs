using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Api;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class ApiSmokeTest : IAsyncLifetime
{
    private ShopApiClient? _shopApiClient;

    public Task InitializeAsync()
    {
        _shopApiClient = ClientFactory.CreateShopApiClient();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Echo_ShouldReturnOk()
    {
        // Act
        var httpResponse = await _shopApiClient!.Echo().EchoAsync();

        // Assert
        _shopApiClient.Echo().AssertEchoSuccessful(httpResponse);
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_shopApiClient);
    }
}
