using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Common.Dsl;

public class ErrorFailureVerification : ResponseVerification<Error.Error, UseCaseContext>
{
    public ErrorFailureVerification(Error.Error error, UseCaseContext context) 
        : base(error, context)
    {
    }

    public ErrorFailureVerification ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = Context.ExpandAliases(expectedMessage);
        var error = Response;
        var errorMessage = error.Message;
        
        errorMessage.ShouldBe(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: '{errorMessage}'");
        
        return this;
    }

    public ErrorFailureVerification FieldErrorMessage(string expectedField, string expectedMessage)
    {
        var expandedExpectedField = Context.ExpandAliases(expectedField);
        var expandedExpectedMessage = Context.ExpandAliases(expectedMessage);
        var error = Response;
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
