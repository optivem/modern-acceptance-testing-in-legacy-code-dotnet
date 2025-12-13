using Optivem.Lang;

namespace Optivem.Lang;

public class Result<TResponse>
{
    public bool Success { get; }
    private readonly TResponse? _value;
    private readonly IReadOnlyCollection<string>? _errors;

    private Result(bool success, TResponse? value, IReadOnlyCollection<string>? errors)
    {
        Success = success;
        _value = value;
        _errors = errors;
    }

    public static Result<TResponse> SuccessResult(TResponse value) => new(true, value, null);

    public static Result<TResponse> FailureResult(IEnumerable<string> errors) => 
        new(false, default, errors.ToList().AsReadOnly());

    public static Result<TResponse> FailureResult(string error) => FailureResult(new List<string> { error });

    public bool IsFailure() => !Success;

    public TResponse GetValue()
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
    public static Result<VoidValue> Success() => Result<VoidValue>.SuccessResult(VoidValue.Empty);
    public static Result<VoidValue> Failure(IEnumerable<string> errors) => Result<VoidValue>.FailureResult(errors);
    public static Result<VoidValue> Failure(string error) => Result<VoidValue>.FailureResult(error);
}
