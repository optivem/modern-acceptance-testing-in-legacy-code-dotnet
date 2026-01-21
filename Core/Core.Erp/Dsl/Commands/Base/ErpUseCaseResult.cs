using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.Util;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;

public class ErpUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, Error, UseCaseContext, TSuccessVerification, ErrorFailureVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse, UseCaseContext>
{
    public ErpUseCaseResult(
        Result<TSuccessResponse, Error> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new ErrorFailureVerification(error, ctx))
    {
    }
}
