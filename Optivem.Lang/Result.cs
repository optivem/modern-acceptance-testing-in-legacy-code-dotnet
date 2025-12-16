namespace Optivem.Lang;

public class Result<T, E>
{
    private bool _success;
    private readonly T? _value;
    private readonly E? _error;

    private Result(bool success, T? value, E? error)
    {
        _success = success;
        _value = value;
        _error = error;
    }

    public static Result<T, E> Success(T value) => new(true, value, default);

    public static Result<T, E> Failure(E error) => new(false, default, error);

    public bool IsSuccess => _success;

    public bool IsFailure => !_success;

    public T Value
    {
        get
        {
            if (!_success)
                throw new InvalidOperationException("Cannot get value from a failed result");
            return _value!;
        }
    }

    public E Error
    {
        get
        {
            if (_success)
                throw new InvalidOperationException("Cannot get error from a successful result");
            return _error!;
        }
    }

    // TODO: VJ: Map Error
}
