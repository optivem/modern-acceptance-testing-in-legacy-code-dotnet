using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public class ClockErrorVerification : ResponseVerification<ClockErrorResponse>
{
    public ClockErrorVerification(ClockErrorResponse error, UseCaseContext context) 
        : base(error, context)
    {
    }
}
