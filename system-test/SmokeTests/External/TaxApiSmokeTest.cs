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

    //[Fact]
    //public void Home_ShouldReturn200OK()
    //{
    //    // Act
    //    var result = _taxApiClient.Home().Home();

    //    // Assert
    //    Assert.True(result.Success, $"Expected successful response but got errors: {string.Join(", ", result.IsFailure ? result.Errors : new List<string>())}");
    //}
}
