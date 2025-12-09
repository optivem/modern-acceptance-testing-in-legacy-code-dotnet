using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp;

public class ErpDsl
{
    private readonly ErpApiDriver _driver;
    private readonly TestContext _context;

    public ErpDsl(ErpApiDriver driver, TestContext context)
    {
        _driver = driver;
        _context = context;
    }

    public CreateProduct CreateProduct() => new(_driver, _context);
}
