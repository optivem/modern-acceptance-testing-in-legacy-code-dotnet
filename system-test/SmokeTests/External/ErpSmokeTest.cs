using Xunit;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpSmokeTest : IDisposable
{
    private readonly SystemDsl _app;

    public ErpSmokeTest()
    {
        _app = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _app.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToErp()
    {
        _app.Erp.GoToErp()
            .Execute()
            .ShouldSucceed();
    }
}
