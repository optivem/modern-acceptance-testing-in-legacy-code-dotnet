using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl;

public class ErpDsl : IDisposable
{
    private readonly ErpDriver _driver;
    private readonly Context _context;

    public ErpDsl(Context context, SystemConfiguration configuration)
    {
        _driver = CreateDriver(configuration);
        _context = context;
    }

    private static ErpDriver CreateDriver(SystemConfiguration configuration)
    {
        return new ErpDriver(configuration.ErpBaseUrl);
    }

    public GoToErp GoToErp() => new(_driver, _context);

    public CreateProduct CreateProduct() => new(_driver, _context);

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
