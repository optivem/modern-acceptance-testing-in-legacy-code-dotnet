using Commons.Dsl;
using Commons.Util;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
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

        internal ExecutionResultBuilder(ShopUseCaseResult<TSuccessResponse, TSuccessVerification> result)
        {
            _result = result;
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
