using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.System.Ui;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests;

public class UiSmokeTest : IAsyncLifetime
{
    private ShopUiClient _shopUiClient = default!;

    public Task InitializeAsync()
    {
        _shopUiClient = ClientFactory.CreateShopUiClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_shopUiClient);
    }

    [Fact]
    public async Task HomePage_ShouldLoadSuccessfully()
    {
        // Act
        await _shopUiClient.OpenHomePageAsync();

        // Assert
        _shopUiClient.AssertHomePageLoaded();
    }
}
