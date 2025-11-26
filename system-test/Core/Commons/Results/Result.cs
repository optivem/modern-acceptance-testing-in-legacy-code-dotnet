namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Results;

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

    public static Result<T> SuccessResult(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false, default, errors.ToList());
    }

    public static Result<T> Failure()
    {
        return new Result<T>(false, default, new List<string>());
    }

    public bool IsFailure => !Success;

    public T Value
    {
        get
        {
            if (!Success)
            {
                throw new InvalidOperationException("Cannot access Value on a failed Result");
            }
            return _value!;
        }
    }

    public IReadOnlyCollection<string> Errors
    {
        get
        {
            if (Success)
            {
                throw new InvalidOperationException("Cannot access Errors on a successful Result");
            }
            return _errors!;
        }
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.SuccessResult(value);
    
    public static Result<object?> Success() => Result<object?>.SuccessResult(null);
    
    public static Result<T> Failure<T>(IEnumerable<string> errors) => Result<T>.Failure(errors);
    
    public static Result<object?> Failure() => Result<object?>.Failure();
}
