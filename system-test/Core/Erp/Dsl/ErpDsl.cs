using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl;

public class ErpDsl : IDisposable
{
    private readonly ErpApiDriver _driver;
    private readonly Context _context;

    public ErpDsl(Context context)
    {
        _driver = DriverFactory.CreateErpApiDriver();
        _context = context;
    }

    public GoToErp GoToErp() => new(_driver, _context);

    public CreateProduct CreateProduct() => new(_driver, _context);

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
