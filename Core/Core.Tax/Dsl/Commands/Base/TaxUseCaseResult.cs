using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public class TaxUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, Error, UseCaseContext, TSuccessVerification, ErrorFailureVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse, UseCaseContext>
{
    public TaxUseCaseResult(
        Result<TSuccessResponse, Error> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new ErrorFailureVerification(error, ctx))
    {
    }
}
