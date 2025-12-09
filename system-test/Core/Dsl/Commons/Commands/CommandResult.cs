using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;
using Optivem.Results;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;

public class CommandResult<TResponse, TVerification>
{
    private readonly Result<TResponse> _result;
    private readonly TestContext _context;
    private readonly Func<TResponse, TestContext, TVerification> _verificationFactory;

    public CommandResult(Result<TResponse> result, TestContext context, Func<TResponse, TestContext, TVerification> verificationFactory)
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
