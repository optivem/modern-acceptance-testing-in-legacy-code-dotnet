using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

public class ShopUseCaseResult<TSuccessResponse, TSuccessVerification>
    : UseCaseResult<TSuccessResponse, SystemError, TSuccessVerification, ErrorFailureVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    public ShopUseCaseResult(
        Result<TSuccessResponse, SystemError> result,
        UseCaseContext context,
        Func<TSuccessResponse, UseCaseContext, TSuccessVerification> verificationFactory)
        : base(result, context, verificationFactory, (error, ctx) => new ErrorFailureVerification(error, ctx))
    {
    }
}
