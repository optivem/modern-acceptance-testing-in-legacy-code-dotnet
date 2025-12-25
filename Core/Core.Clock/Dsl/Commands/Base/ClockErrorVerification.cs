using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Testing.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public class ClockErrorVerification : ResponseVerification<ClockErrorResponse, UseCaseContext>
{
    public ClockErrorVerification(ClockErrorResponse error, UseCaseContext context) 
        : base(error, context)
    {
    }

    public ClockErrorVerification ErrorMessage(string expectedMessage)
    {
        var expandedExpectedMessage = Context.ExpandAliases(expectedMessage);
        var errorMessage = Response.Message;
        
        errorMessage.ShouldBe(expandedExpectedMessage, 
            $"Expected error message: '{expandedExpectedMessage}', but got: '{errorMessage}'");
        
        return this;
    }
}
