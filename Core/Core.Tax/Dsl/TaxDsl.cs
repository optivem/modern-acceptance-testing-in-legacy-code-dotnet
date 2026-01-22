using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl;

public class TaxDsl : IDisposable
{
    private readonly ITaxDriver _driver;
    private readonly UseCaseContext _context;

    private TaxDsl(ITaxDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public TaxDsl(string baseUrl, UseCaseContext context)
        : this(CreateDriver(baseUrl, context), context)
    {
    }

    private static ITaxDriver CreateDriver(string baseUrl, UseCaseContext context)
    {
        return context.ExternalSystemMode switch
        {
            ExternalSystemMode.Real => new TaxRealDriver(baseUrl),
            ExternalSystemMode.Stub => new TaxStubDriver(baseUrl),
            _ => throw new ArgumentOutOfRangeException($"Unknown mode: {context.ExternalSystemMode}")
        };
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToTax GoToTax() => new(_driver, _context);
    public ReturnsTaxRate ReturnsTaxRate() => new(_driver, _context); 
    public GetTaxRate GetTaxRate() => new(_driver, _context);
}
