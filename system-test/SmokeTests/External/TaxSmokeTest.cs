using Xunit;
using Optivem.EShop.SystemTest.Core;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxSmokeTest : IDisposable
{
    private readonly SystemDsl _app;

    public TaxSmokeTest()
    {
        _app = SystemDslFactory.Create();
    }

    public void Dispose()
    {
        _app.Dispose();
    }

    [Fact]
    public void ShouldBeAbleToGoToTax()
    {
        _app.Tax.GoToTax()
            .Execute()
            .ShouldSucceed();
    }

}
