using Optivem.Lang;

namespace Optivem.Testing.Dsl;

public class UseCaseResult<TSuccessResponse, TFailureResponse, TContext, TSuccessVerification, TFailureVerification>
{
    private readonly Result<TSuccessResponse, TFailureResponse> _result;
    private readonly TContext _context;
    private readonly Func<TSuccessResponse, TContext, TSuccessVerification> _verificationFactory;
    private readonly Func<TFailureResponse, TContext, TFailureVerification> _failureVerificationFactory;

    public UseCaseResult(
        Result<TSuccessResponse, TFailureResponse> result, 
        TContext context, 
        Func<TSuccessResponse, TContext, TSuccessVerification> verificationFactory,
        Func<TFailureResponse, TContext, TFailureVerification> failureVerificationFactory)
    {
        _result = result;
        _context = context;
        _verificationFactory = verificationFactory;
        _failureVerificationFactory = failureVerificationFactory;
    }

    public TSuccessVerification ShouldSucceed()
    {
        if (!_result.IsSuccess)
        {
            throw new InvalidOperationException($"Expected result to be success but was failure");
        }

        return _verificationFactory(_result.Value, _context);
    }

    public TFailureVerification ShouldFail()
    {
        if (_result.IsSuccess)
        {
            throw new InvalidOperationException("Expected result to be failure but was success");
        }

        return _failureVerificationFactory(_result.Error, _context);
    }
}
