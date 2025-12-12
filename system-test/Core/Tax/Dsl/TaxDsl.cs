using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl;

public class TaxDsl : IDisposable
{
    private readonly TaxApiDriver _driver;
    private readonly Context _context;

    public TaxDsl(Context context)
    {
        _driver = DriverFactory.CreateTaxApiDriver();
        _context = context;
    }

    public GoToTax GoToTax() => new(_driver, _context);

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
