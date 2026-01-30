using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public abstract class BaseClockCommand<TResponse, TVerification> 
    where TVerification : ResponseVerification<TResponse>
{
    protected readonly IClockDriver _driver;
    protected readonly UseCaseContext _context;

    protected BaseClockCommand(IClockDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract Task<ClockUseCaseResult<TResponse, TVerification>> Execute();
}
