using Xunit;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxSmokeTest : IDisposable
{
    private readonly SystemDsl _dsl;

    public TaxSmokeTest()
    {
        _dsl = SystemDslFactory.Create();
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
