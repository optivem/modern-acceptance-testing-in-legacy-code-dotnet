namespace Optivem.Testing.Dsl;

public abstract class BaseUseCase<TDriver, TContext, TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> 
    : IUseCase<UseCaseResult<TSuccessResponse, TFailureResponse, TContext, TSuccessVerification, TFailureVerification>>
{
    protected readonly TDriver _driver;
    protected readonly TContext _context;

    protected BaseUseCase(TDriver driver, TContext context)
    {
        _driver = driver;
        _context = context;
    }

    public abstract UseCaseResult<TSuccessResponse, TFailureResponse, TContext, TSuccessVerification, TFailureVerification> Execute();
}
