namespace Optivem.Commons.Dsl;

public abstract class BaseUseCase<TDriver, TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> 
    : IUseCase<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>>
{
    protected readonly TDriver _driver;
    protected readonly UseCaseContext _context;

    protected BaseUseCase(TDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> Execute();
}
