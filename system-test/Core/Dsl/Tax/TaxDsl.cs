using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Tax.Commands;

namespace Optivem.EShop.SystemTest.Core.Dsl.Tax;

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
