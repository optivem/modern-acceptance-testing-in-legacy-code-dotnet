using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin.Then;
using Optivem.Testing;

namespace Dsl.Gherkin.Then
{
    public class ThenClause<TSuccessResponse, TSuccessVerification> : BaseClause
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly SystemDsl _app;
        private readonly ScenarioDsl _scenario;
        private readonly ExecutionResult<TSuccessResponse, TSuccessVerification> _executionResult;

        public ThenClause(Channel channel, SystemDsl app, ScenarioDsl scenario, ExecutionResult<TSuccessResponse, TSuccessVerification> executionResult)
            : base(channel)
        {
            _app = app;
            _scenario = scenario;
            _executionResult = executionResult;
        }

        public ThenSuccessBuilder<TSuccessResponse, TSuccessVerification> ShouldSucceed()
        {
            if (_executionResult == null)
            {
                throw new InvalidOperationException("Cannot verify success: no operation was executed");
            }
            _scenario.MarkAsExecuted();
            var successVerification = _executionResult.Result.ShouldSucceed();
            return new ThenSuccessBuilder<TSuccessResponse, TSuccessVerification>(this, successVerification);
        }

        public ThenFailureBuilder<TSuccessResponse, TSuccessVerification> ShouldFail()
        {
            _scenario.MarkAsExecuted();
            return new ThenFailureBuilder<TSuccessResponse, TSuccessVerification>(this, _executionResult.Result);
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> Order(string orderNumber)
        {
            _scenario.MarkAsExecuted();
            return new ThenOrderBuilder<TSuccessResponse, TSuccessVerification>(this, _app, orderNumber);
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> Order()
        {
            var orderNumber = _executionResult.OrderNumber;

            if (orderNumber == null)
            {
                throw new InvalidOperationException("Cannot verify order: no order number available from the executed operation");
            }

            return Order(orderNumber);
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> Coupon(string couponCode)
        {
            _scenario.MarkAsExecuted();
            return new ThenCouponBuilder<TSuccessResponse, TSuccessVerification>(this, _app, couponCode);
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> Coupon()
        {
            var couponCode = _executionResult.CouponCode;

            if (couponCode == null)
            {
                throw new InvalidOperationException("Cannot verify coupon: no coupon code available from the executed operation");
            }

            return Coupon(couponCode);
        }
    }
}