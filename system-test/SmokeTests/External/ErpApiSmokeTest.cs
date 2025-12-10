using Optivem.EShop.SystemTest.Core.Dsl;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpApiSmokeTest : IDisposable
{
    private readonly Dsl _dsl;

    public ErpApiSmokeTest()
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
