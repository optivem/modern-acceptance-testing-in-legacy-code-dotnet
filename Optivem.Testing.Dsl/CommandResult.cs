using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.Testing.Dsl;

public class CommandResult<TResponse, TVerification>
{
    private readonly Result<TResponse, Error> _result;
    private readonly Context _context;
    private readonly Func<TResponse, Context, TVerification> _verificationFactory;

    public CommandResult(Result<TResponse, Error> result, Context context, Func<TResponse, Context, TVerification> verificationFactory)
    {
        _result = result;
        _context = context;
        _verificationFactory = verificationFactory;
    }

    public TVerification ShouldSucceed()
    {
        if (!_result.IsSuccess)
        {
            var error = _result.Error;
            throw new InvalidOperationException($"Expected result to be success but was failure with error: {error.Message}");
        }

        return _verificationFactory(_result.Value, _context);
    }

    public FailureVerification<TResponse> ShouldFail()
    {
        if (_result.IsSuccess)
        {
            throw new InvalidOperationException("Expected result to be failure but was success");
        }

        return new FailureVerification<TResponse>(_result, _context);
    }
}
