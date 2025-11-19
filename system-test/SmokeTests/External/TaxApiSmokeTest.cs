using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Tax;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests.External;

public class TaxApiSmokeTest : IAsyncLifetime
{
    private TaxApiClient _taxApiClient = default!;

    public Task InitializeAsync()
    {
        _taxApiClient = ClientFactory.CreateTaxApiClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_taxApiClient);
    }

    [Fact]
    public async Task Home_ShouldReturn200OK()
    {
        // Act
        var httpResponse = await _taxApiClient.Home().HomeAsync();

        // Assert
        _taxApiClient.Home().AssertHomeSuccessful(httpResponse);
    }
}
