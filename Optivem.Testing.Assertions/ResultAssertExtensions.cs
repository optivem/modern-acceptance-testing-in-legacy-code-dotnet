using Shouldly;
using Optivem.Results;

namespace Optivem.Testing.Assertions;

public static class ResultAssertExtensions
{
    public static Result<T> ShouldBeSuccess<T>(this Result<T> result)
    {
        if (!result.Success)
        {
            var errors = string.Join(", ", result.GetErrors());
            throw new ShouldAssertException($"Expected result to be success but was failure with errors: {errors}");
        }
        return result;
    }

    public static Result<T> ShouldBeFailure<T>(this Result<T> result)
    {
        result.IsFailure().ShouldBeTrue("Expected result to be failure but was success");
        return result;
    }

    public static Result<T> ShouldBeFailure<T>(this Result<T> result, string errorMessage)
    {
        result.ShouldBeFailure();
        result.GetErrors().ShouldContain(errorMessage, 
            $"Expected result to contain error '{errorMessage}' but errors were: {string.Join(", ", result.GetErrors())}");
        return result;
    }
}
