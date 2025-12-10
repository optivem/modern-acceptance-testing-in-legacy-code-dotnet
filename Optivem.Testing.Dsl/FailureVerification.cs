using Optivem.Results;
using Shouldly;

namespace Optivem.Testing.Dsl;

public class FailureVerification
{
    private readonly Result<object> _result;
    private readonly Context _context;

    public FailureVerification(Result<object> result, Context context)
    {
        _result = result;
        _context = context;
    }

    public FailureVerification ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = _context.ExpandAliases(expectedMessage);
        var errors = _result.GetErrors();
        errors.ShouldContain(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: {string.Join(", ", errors)}");
        return this;
    }
}
