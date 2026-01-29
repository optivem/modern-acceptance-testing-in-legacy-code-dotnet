using Commons.Dsl;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsl.Gherkin
{
    public class ExecutionResultBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly ShopUseCaseResult<TSuccessResponse, TSuccessVerification> _result;
        private string? _orderNumber;
        private string? _couponCode;

        internal ExecutionResultBuilder(UseCaseResult<TSuccessResponse, SystemError, TSuccessVerification, ErrorFailureVerification> result)
        {
            // Cast to derived type - the result is always a ShopUseCaseResult at runtime
            _result = (ShopUseCaseResult<TSuccessResponse, TSuccessVerification>)result;
        }

        public ExecutionResultBuilder<TSuccessResponse, TSuccessVerification> OrderNumber(string? orderNumber)
        {
            _orderNumber = orderNumber;
            return this;
        }

        public ExecutionResultBuilder<TSuccessResponse, TSuccessVerification> CouponCode(string? couponCode)
        {
            _couponCode = couponCode;
            return this;
        }

        public ExecutionResult<TSuccessResponse, TSuccessVerification> Build()
        {
            return new ExecutionResult<TSuccessResponse, TSuccessVerification>(
                _result,
                _orderNumber,
                _couponCode);
        }
    }
}
