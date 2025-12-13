using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.Testing.Dsl;

public class CommandResult<TResponse, TVerification>
{
    private readonly Result<TResponse> _result;
    private readonly Context _context;
    private readonly Func<TResponse, Context, TVerification> _verificationFactory;

    public CommandResult(Result<TResponse> result, Context context, Func<TResponse, Context, TVerification> verificationFactory)
    {
        _result = result;
        _context = context;
        _verificationFactory = verificationFactory;
    }

    public TVerification ShouldSucceed()
    {
        if (!_result.Success)
        {
            var errors = string.Join(", ", _result.GetErrors());
            throw new InvalidOperationException($"Expected result to be success but was failure with errors: {errors}");
        }

        return _verificationFactory(_result.GetValue(), _context);
    }

    public FailureVerification ShouldFail()
    {
        if (_result.Success)
        {
            throw new InvalidOperationException("Expected result to be failure but was success");
        }

        var objectResult = Result<object>.FailureResult(_result.GetErrors());
        return new FailureVerification(objectResult, _context);
    }
}
