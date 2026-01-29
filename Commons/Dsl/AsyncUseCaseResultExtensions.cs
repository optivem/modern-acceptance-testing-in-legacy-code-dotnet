using System.Runtime.CompilerServices;

namespace Commons.Dsl;

public static class AsyncUseCaseResultExtensions
{
    public static AsyncSuccessVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> ShouldSucceed<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>(
        this Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> taskResult)
    {
        return new AsyncSuccessVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>(taskResult);
    }

    public static AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> ShouldFail<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>(
        this Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> taskResult)
    {
        return new AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>(taskResult);
    }
}

public class AsyncSuccessVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>
{
    private readonly Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> _taskResult;
    private readonly List<Action<TSuccessVerification>> _verifications = new();

    public AsyncSuccessVerificationBuilder(Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> taskResult)
    {
        _taskResult = taskResult;
    }

    private async Task<TSuccessVerification> ExecuteVerifications()
    {
        var result = await _taskResult;
        var verification = result.ShouldSucceed();
        
        foreach (var verify in _verifications)
        {
            verify(verification);
        }
        
        return verification;
    }

    public TaskAwaiter<TSuccessVerification> GetAwaiter()
    {
        return ExecuteVerifications().GetAwaiter();
    }

    public static implicit operator Task<TSuccessVerification>(AsyncSuccessVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> builder)
    {
        return builder.ExecuteVerifications();
    }

    // Forward verification methods to the underlying verification type through reflection/dynamic
    // This is a simplified version - in practice, you'd need to handle all verification methods
    // For now, we'll just make it awaitable and handle verifications in the actual verification classes
}

public class AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>
{
    private readonly Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> _taskResult;
    private readonly List<Action<TFailureVerification>> _verifications = new();

    public AsyncFailureVerificationBuilder(Task<UseCaseResult<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification>> taskResult)
    {
        _taskResult = taskResult;
    }

    private async Task<TFailureVerification> ExecuteVerifications()
    {
        var result = await _taskResult;
        var verification = result.ShouldFail();
        
        foreach (var verify in _verifications)
        {
            verify(verification);
        }
        
        return verification;
    }

    public AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> ErrorMessage(string expectedMessage)
    {
        _verifications.Add(v => 
        {
            // Use dynamic to call ErrorMessage regardless of the actual type
            dynamic dynamicVerification = v!;
            dynamicVerification.ErrorMessage(expectedMessage);
        });
        return this;
    }

    public AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> FieldErrorMessage(string field, string expectedMessage)
    {
        _verifications.Add(v => 
        {
            // Use dynamic to call FieldErrorMessage regardless of the actual type
            dynamic dynamicVerification = v!;
            dynamicVerification.FieldErrorMessage(field, expectedMessage);
        });
        return this;
    }

    public TaskAwaiter<TFailureVerification> GetAwaiter()
    {
        return ExecuteVerifications().GetAwaiter();
    }

    public static implicit operator Task<TFailureVerification>(AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> builder)
    {
        return builder.ExecuteVerifications();
    }

    public static implicit operator Task(AsyncFailureVerificationBuilder<TSuccessResponse, TFailureResponse, TSuccessVerification, TFailureVerification> builder)
    {
        return builder.ExecuteVerifications();
    }
}

public interface IErrorVerification
{
    void ErrorMessage(string expectedMessage);
    void FieldErrorMessage(string field, string expectedMessage);
}
