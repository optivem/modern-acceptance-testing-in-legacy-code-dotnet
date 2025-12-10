using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp;

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
