using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public class ClockErrorVerification : ResponseVerification<ClockErrorResponse, UseCaseContext>
{
    public ClockErrorVerification(ClockErrorResponse error, UseCaseContext context) 
        : base(error, context)
    {
    }
}
