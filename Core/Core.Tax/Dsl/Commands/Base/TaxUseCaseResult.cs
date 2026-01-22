using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Verifications;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;

public class TaxUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, TaxErrorResponse, UseCaseContext, TSuccessVerification, TaxErrorVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse, UseCaseContext>
{
    public TaxUseCaseResult(
        Result<TSuccessResponse, TaxErrorResponse> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new TaxErrorVerification(error, ctx))
    {
    }
}
