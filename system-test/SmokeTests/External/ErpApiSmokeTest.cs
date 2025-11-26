using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests.External;

public class ErpApiSmokeTest : IAsyncLifetime
{
    private ErpApiClient _erpApiClient = default!;

    public Task InitializeAsync()
    {
        _erpApiClient = ClientFactory.CreateErpApiClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ClientCloser.CloseAsync(_erpApiClient);
    }

    //[Fact]
    //public void Home_ShouldReturn200OK()
    //{
    //    // Act
    //    var result = _erpApiClient.Home().Home();

    //    // Assert
    //    Assert.True(result.Success, $"Expected successful response but got errors: {string.Join(", ", result.IsFailure ? result.Errors : new List<string>())}");
    //}
}
