using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Drivers.Commons;

public static class ResultExtensions
{
    public static Result<T> ShouldBeSuccess<T>(this Result<T> result)
    {
        result.Success.ShouldBeTrue($"Expected result to be success but was failure with errors: {string.Join(", ", result.GetErrors())}");
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
