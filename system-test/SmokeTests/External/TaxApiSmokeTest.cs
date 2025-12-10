using Optivem.EShop.SystemTest.Core.Dsl;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxApiSmokeTest : IDisposable
{
    private readonly Dsl _dsl;
    
    public TaxApiSmokeTest()
    {
        _dsl = new Dsl();
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
