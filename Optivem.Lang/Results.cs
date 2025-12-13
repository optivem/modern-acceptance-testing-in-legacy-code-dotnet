namespace Optivem.Lang;

public static class Results
{
    public static Result<T, Error> Success<T>(T value)
    {
        return Result<T, Error>.SuccessResult(value);
    }

    public static Result<VoidValue, Error> Success()
    {
        return Result<VoidValue, Error>.SuccessResult(VoidValue.Empty);
    }

    public static Result<T, Error> Failure<T>(string message)
    {
        return Result<T, Error>.FailureResult(Error.Of(message));
    }

    public static Result<T, Error> Failure<T>(string message, params Error.FieldError[] fieldErrors)
    {
        return Result<T, Error>.FailureResult(Error.Of(message, fieldErrors));
    }

    public static Result<T, Error> Failure<T>(Error error)
    {
        return Result<T, Error>.FailureResult(error);
    }
}
