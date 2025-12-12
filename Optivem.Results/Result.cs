namespace Optivem.Results;

public class Result<T>
{
    public bool Success { get; }
    private readonly T? _value;
    private readonly IReadOnlyCollection<string>? _errors;

    private Result(bool success, T? value, IReadOnlyCollection<string>? errors)
    {
        Success = success;
        _value = value;
        _errors = errors;
    }

    public static Result<T> SuccessResult(T value) => new(true, value, null);

    public static Result<T> FailureResult(IEnumerable<string> errors) => 
        new(false, default, errors.ToList().AsReadOnly());

    public static Result<T> FailureResult(string error) => FailureResult(new List<string> { error });

    public bool IsFailure() => !Success;

    public T GetValue()
    {
        if (!Success)
            throw new InvalidOperationException("Cannot get value from a failed result");
        return _value!;
    }

    public IReadOnlyCollection<string> GetErrors()
    {
        if (Success)
            throw new InvalidOperationException("Cannot get error from a successful result");
        return _errors!;
    }
}

public static class Result
{
    public static Result<VoidResult> Success() => Result<VoidResult>.SuccessResult(new VoidResult());
    public static Result<VoidResult> Failure(IEnumerable<string> errors) => Result<VoidResult>.FailureResult(errors);
    public static Result<VoidResult> Failure(string error) => Result<VoidResult>.FailureResult(error);
}

public record VoidResult;
