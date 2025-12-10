using Optivem.EShop.SystemTest.Core.Dsl;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpSmokeTest : IDisposable
{
    private readonly Dsl _dsl;

    public ErpSmokeTest()
    {
        _dsl = new Dsl();
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
