using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.Results;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;

public class FailureVerification
{
    private readonly Result<object> _result;
    private readonly TestContext _context;

    public FailureVerification(Result<object> result, TestContext context)
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
