using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Tax;
using Xunit;

namespace Optivem.EShop.SystemTest.SmokeTests.External;

public class TaxApiSmokeTest : IDisposable
{
    private readonly Context _context;
    private readonly TaxDsl _tax;
    
    public TaxApiSmokeTest()
    {
        _context = new Context();
        _tax = new TaxDsl(_context);
    }
    
    [Fact]
    public void ShouldBeAbleToGoToTax()
    {
        _tax.GoToTax()
            .Execute()
            .ShouldSucceed();
    }

    public void Dispose()
    {
        _tax?.Dispose();
    }
}
