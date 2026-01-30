using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public abstract class BaseTaxCommand<TResponse, TVerification>
    where TVerification : ResponseVerification<TResponse>
{
    protected readonly ITaxDriver _driver;
    protected readonly UseCaseContext _context;

    protected BaseTaxCommand(ITaxDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract Task<TaxUseCaseResult<TResponse, TVerification>> Execute();
}
