using Commons.Util;

namespace Commons.Dsl;

public class UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>
{
    private readonly Result<TSuccessResponse, TFailureResponse> _result;
    private readonly UseCaseContext _context;
    private readonly Func<TSuccessResponse, UseCaseContext, TSuccessVerification> _verificationFactory;
    private readonly Func<TFailureResponse, UseCaseContext, TFailureVerification> _failureVerificationFactory;

    public UseCaseResult(
        Result<TSuccessResponse, TFailureResponse> result,
        UseCaseContext context, 
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory,
        Func<TFailureResponse, UseCaseContext, TFailureVerification> failureVerificationFactory)
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
            throw new InvalidOperationException($"Expected result to be success but was failure, due to error: " + _result.Error!.ToString());
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
