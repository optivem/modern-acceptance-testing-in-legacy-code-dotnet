using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Base;

namespace SmokeTests.External;

public class TaxSmokeTest : BaseSystemTest
{
    [Fact]
    public void ShouldBeAbleToGoToTax()
    {
        App.Tax().GoToTax()
            .Execute()
            .ShouldSucceed();
    }
}
