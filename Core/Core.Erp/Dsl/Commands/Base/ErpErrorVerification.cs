using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public class ErpErrorVerification : ResponseVerification<ErpErrorResponse>
{
    public ErpErrorVerification(ErpErrorResponse error, UseCaseContext context) 
        : base(error, context)
    {
    }

    public ErpErrorVerification ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = Context.ExpandAliases(expectedMessage);
        var error = Response;
        var errorMessage = error.Message;
        
        errorMessage.ShouldBe(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: '{errorMessage}'");
        
        return this;
    }
}