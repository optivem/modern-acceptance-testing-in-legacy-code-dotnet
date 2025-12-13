using Shouldly;
using Optivem.Lang;

namespace Optivem.Testing.Assertions;

public static class ResultAssertExtensions
{
    public static Result<T, E> ShouldBeSuccess<T, E>(this Result<T, E> result)
    {
        if (!result.Success)
        {
            var error = result.GetError();
            throw new ShouldAssertException($"Expected result to be success but was failure with error: {error}");
        }
        return result;
    }

    public static Result<T, E> ShouldBeFailure<T, E>(this Result<T, E> result)
    {
        result.IsFailure().ShouldBeTrue("Expected result to be failure but was success");
        return result;
    }

    public static Result<T, Error> ShouldBeFailure<T>(this Result<T, Error> result, string errorMessage)
    {
        result.ShouldBeFailure();
        var error = result.GetError();
        error.Message.ShouldBe(errorMessage, 
            $"Expected result to contain error message '{errorMessage}' but got: {error.Message}");
        return result;
    }
}
