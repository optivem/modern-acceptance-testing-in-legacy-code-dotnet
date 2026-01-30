using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public abstract class BaseErpCommand<TResponse, TVerification>
    where TVerification : ResponseVerification<TResponse>
{
    protected readonly IErpDriver _driver;
    protected readonly UseCaseContext _context;

    protected BaseErpCommand(IErpDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract Task<ErpUseCaseResult<TResponse, TVerification>> Execute();
}
