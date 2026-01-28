using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Commons.Util;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public class ErpUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, ErpErrorResponse, TSuccessVerification, ErpErrorVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    public ErpUseCaseResult(
        Result<TSuccessResponse, ErpErrorResponse> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new ErpErrorVerification(error, ctx))
    {
    }
}
