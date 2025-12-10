using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Erp;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class ErpApiSmokeTest : IDisposable
{
    private readonly Context _context;
    private readonly ErpDsl _erp;

    public ErpApiSmokeTest()
    {
        _context = new Context();
        _erp = new ErpDsl(_context);
    }

    [Fact]
    public void ShouldBeAbleToGoToErp()
    {
        _erp.GoToErp()
            .Execute()
            .ShouldSucceed();
    }

    public void Dispose()
    {
        _erp?.Dispose();
    }
}
