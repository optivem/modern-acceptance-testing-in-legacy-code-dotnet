using Commons.Util;
using Optivem.EShop.SystemTest.Base.V4;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.External;

public class TaxSmokeTest : BaseChannelDriverTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToTax()
    {
        var result = await _taxDriver!.GoToTax();
        result.ShouldBeSuccess();
    }
}
