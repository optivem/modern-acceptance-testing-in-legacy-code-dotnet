using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl;

public class TaxDsl : IDisposable
{
    private readonly TaxDriver _driver;
    private readonly UseCaseContext _context;

    public TaxDsl(UseCaseContext context, SystemConfiguration configuration)
    {
        _driver = CreateDriver(configuration);
        _context = context;
    }

    private static TaxDriver CreateDriver(SystemConfiguration configuration)
    {
        return new TaxDriver(configuration.TaxBaseUrl);
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToTax GoToTax() => new(_driver, _context);
}
