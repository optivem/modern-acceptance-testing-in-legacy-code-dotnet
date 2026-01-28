using Optivem.Commons.Dsl;
using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;

namespace Dsl.Gherkin
{
    public class ExecutionResult<TSuccessResponse, TSuccessVerification> 
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        internal ExecutionResult(ShopUseCaseResult<TSuccessResponse, TSuccessVerification> result, 
            string? orderNumber, string? couponCode)
        {
            if (result == null)
            {
                throw new ArgumentException("Result cannot be null");
            }

            Result = result;
            OrderNumber = orderNumber;
            CouponCode = couponCode;
        }

        public ShopUseCaseResult<TSuccessResponse, TSuccessVerification> Result { get; }

        public string? OrderNumber { get; }

        public string? CouponCode { get; }
    }
}