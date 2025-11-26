using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.External;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Clients.Commons;

namespace Optivem.AtddAccelerator.EShop.SystemTest.SmokeTests.External;

public class ErpApiSmokeTest
{
    private ErpApiDriver _erpApiDriver = default!;

    public ErpApiSmokeTest()
    {
        _erpApiDriver = DriverFactory.CreateErpApiDriver();
    }

    //public void Dispose()
    //{
    //    Closer.Close(_erpApiDriver);
    //}

    //[Fact]
    //public void ShouldBeAbleToGoToErp()
    //{
    //    var result = _shopApiDriver.GoToShop();
    //    Assert.True(result.Success);
    //}



    //[Fact]
    //public void Home_ShouldReturn200OK()
    //{
    //    // Act
    //    var result = _erpApiClient.Home().Home();

    //    // Assert
    //    Assert.True(result.Success, $"Expected successful response but got errors: {string.Join(", ", result.IsFailure ? result.Errors : new List<string>())}");
    //}
}
