using Xunit;
using TestDsl = global::Optivem.EShop.SystemTest.Dsl.Dsl;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxSmokeTest : IDisposable
{
    private readonly TestDsl _dsl;

    public TaxSmokeTest()
    {
        _dsl = new TestDsl();
    }

    public void Dispose()
    {
        _dsl.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToTax()
    {
        _dsl.Tax.GoToTax()
            .Execute()
            .ShouldSucceed();
    }

}
