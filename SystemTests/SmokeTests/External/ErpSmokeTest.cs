using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests.External;

public class ErpSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        (await App.Erp().GoToErp()
            .Execute())
            .ShouldSucceed();
    }
}
