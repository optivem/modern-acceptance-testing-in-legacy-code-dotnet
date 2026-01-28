using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl;

public class ErpDsl : IDisposable
{
    private readonly IErpDriver _driver;
    private readonly UseCaseContext _context;

    private ErpDsl(IErpDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public ErpDsl(string baseUrl, UseCaseContext context)
        : this(CreateDriver(baseUrl, context), context)
    {
    }

    private static IErpDriver CreateDriver(string baseUrl, UseCaseContext context)
    {
        return context.ExternalSystemMode switch
        {
            ExternalSystemMode.Real => new ErpRealDriver(baseUrl),
            ExternalSystemMode.Stub => new ErpStubDriver(baseUrl),
            _ => throw new InvalidOperationException($"Unknown external system mode: {context.ExternalSystemMode}")
        };
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToErp GoToErp() => new(_driver, _context);

    public ReturnsProduct ReturnsProduct() => new(_driver, _context);

    public GetProduct GetProduct() => new(_driver, _context);
}
