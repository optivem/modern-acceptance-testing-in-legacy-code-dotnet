using Optivem.Lang;
using Shouldly;

namespace Optivem.Testing.Dsl;

public class FailureVerification<T>
{
    private readonly Result<T, Error> _result;
    private readonly Context _context;

    public FailureVerification(Result<T, Error> result, Context context)
    {
        _result = result;
        _context = context;
    }

    public FailureVerification<T> ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = _context.ExpandAliases(expectedMessage);
        var error = _result.GetError();
        var errorMessage = error.Message;
        
        errorMessage.ShouldBe(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: '{errorMessage}'");
        
        return this;
    }

    public FailureVerification<T> FieldErrorMessage(string expectedField, string expectedMessage)
    {
        var expandedExpectedField = _context.ExpandAliases(expectedField);
        var expandedExpectedMessage = _context.ExpandAliases(expectedMessage);
        var error = _result.GetError();
        var fields = error.Fields;

        fields.ShouldNotBeNull("Expected field errors but none were found");
        fields.ShouldNotBeEmpty("Expected field errors but none were found");

        var matchingFieldError = fields.FirstOrDefault(f => expandedExpectedField.Equals(f.Field));

        matchingFieldError.ShouldNotBeNull(
            $"Expected field error for field '{expandedExpectedField}', but field was not found in errors: {string.Join(", ", fields.Select(f => f.Field))}");

        var actualMessage = matchingFieldError.Message;
        actualMessage.ShouldBe(expandedExpectedMessage,
            $"Expected field error message for field '{expandedExpectedField}': '{expandedExpectedMessage}', but got: '{actualMessage}'");

        return this;
    }
}
