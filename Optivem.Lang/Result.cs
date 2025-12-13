namespace Optivem.Lang;

public class Result<T, E>
{
    public bool Success { get; }
    private readonly T? _value;
    private readonly E? _error;

    private Result(bool success, T? value, E? error)
    {
        Success = success;
        _value = value;
        _error = error;
    }

    public static Result<T, E> SuccessResult(T value) => new(true, value, default);

    public static Result<T, E> FailureResult(E error) => new(false, default, error);

    public bool IsFailure() => !Success;

    public T GetValue()
    {
        if (!Success)
            throw new InvalidOperationException("Cannot get value from a failed result");
        return _value!;
    }

    public E GetError()
    {
        if (Success)
            throw new InvalidOperationException("Cannot get error from a successful result");
        return _error!;
    }
}
