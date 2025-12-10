using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp;

public class ErpDsl
{
    private readonly ErpApiDriver _driver;
    private readonly Context _context;

    public ErpDsl(ErpApiDriver driver, Context context)
    {
        _driver = driver;
        _context = context;
    }

    public CreateProduct CreateProduct() => new(_driver, _context);
}
