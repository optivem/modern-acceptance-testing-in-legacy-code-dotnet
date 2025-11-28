using FluentAssertions;

namespace Optivem.EShop.SystemTest.Core.Drivers.Commons;

public static class ResultAssert
{
    public static ResultAssertion<T> AssertThatResult<T>(Result<T> actual) => new(actual);
}

public class ResultAssertion<T>
{
    private readonly Result<T> _actual;

    public ResultAssertion(Result<T> actual)
    {
        _actual = actual;
    }

    public ResultAssertion<T> IsSuccess()
    {
        if (!_actual.Success)
        {
            throw new Exception($"Expected result to be success but was failure with errors: {string.Join(", ", _actual.GetErrors())}");
        }
        return this;
    }

    public ResultAssertion<T> IsFailure()
    {
        if (!_actual.IsFailure())
        {
            throw new Exception("Expected result to be failure but was success");
        }
        return this;
    }

    public ResultAssertion<T> IsFailure(string errorMessage)
    {
        IsFailure();
        if (!_actual.GetErrors().Contains(errorMessage))
        {
            throw new Exception($"Expected result to contain error '{errorMessage}' but errors were: {string.Join(", ", _actual.GetErrors())}");
        }
        return this;
    }
}
