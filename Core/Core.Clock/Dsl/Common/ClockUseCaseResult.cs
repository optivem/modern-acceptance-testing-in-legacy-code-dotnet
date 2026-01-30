using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Util;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public class ClockUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, ClockErrorResponse, TSuccessVerification, ClockErrorVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    public ClockUseCaseResult(
        Result<TSuccessResponse, ClockErrorResponse> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new ClockErrorVerification(error, ctx))
    {
    }
}
