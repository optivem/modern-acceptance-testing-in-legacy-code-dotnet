using Xunit;
using TestDsl = global::Optivem.EShop.SystemTest.Dsl.Dsl;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpSmokeTest : IDisposable
{
    private readonly TestDsl _dsl;

    public ErpSmokeTest()
    {
        _dsl = new TestDsl();
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
