using Xunit;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpSmokeTest : IDisposable
{
    private readonly SystemDsl _dsl;

    public ErpSmokeTest()
    {
        _dsl = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _dsl.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToErp()
    {
        _dsl.Erp.GoToErp()
            .Execute()
            .ShouldSucceed();
    }
}
