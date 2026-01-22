using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public class TaxErrorVerification : ResponseVerification<TaxErrorResponse, UseCaseContext>
{
    public TaxErrorVerification(TaxErrorResponse error, UseCaseContext context) 
        : base(error, context)
    {
    }

    public TaxErrorVerification ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = Context.ExpandAliases(expectedMessage);
        var error = Response;
        var errorMessage = error.Message;
        
        errorMessage.ShouldBe(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: '{errorMessage}'");
        
        return this;
    }
}