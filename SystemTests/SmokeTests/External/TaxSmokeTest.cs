using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests.External;

public class TaxSmokeTest : BaseSystemTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        (await App.Tax().GoToTax()
            .Execute())
            .ShouldSucceed();
    }
}
