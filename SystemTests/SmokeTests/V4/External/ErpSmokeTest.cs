using Commons.Util;
using Optivem.EShop.SystemTest.Base.V4;
using Shouldly;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.V4.External;

public class ErpSmokeTest : BaseChannelDriverTest
{
    [Fact]
    public async Task ShouldBeAbleToGoToErp()
    {
        var result = await _erpDriver!.GoToErp();
        result.ShouldBeSuccess();
    }
}
