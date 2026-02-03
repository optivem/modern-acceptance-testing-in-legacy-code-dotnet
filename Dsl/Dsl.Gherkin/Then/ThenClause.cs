using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin.Then;
using Optivem.Testing;

namespace Dsl.Gherkin.Then
{
    public class ThenClause<TSuccessResponse, TSuccessVerification> : BaseClause
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly SystemDsl _app;
        private readonly Func<Task<ExecutionResult<TSuccessResponse, TSuccessVerification>>> _lazyExecute;
        private ExecutionResult<TSuccessResponse, TSuccessVerification>? _executionResult;
        private bool _executionCompleted = false;

        public ThenClause(Channel channel, SystemDsl app, Func<Task<ExecutionResult<TSuccessResponse, TSuccessVerification>>> lazyExecute)
            : base(channel)
        {
            _app = app;
            _lazyExecute = lazyExecute;
        }

        public async Task<ThenSuccessBuilder<TSuccessResponse, TSuccessVerification>> ShouldSucceed()
        {
            var result = await GetExecutionResult();
            if (result == null)
            {
                throw new InvalidOperationException("Cannot verify success: no operation was executed");
            }
            var successVerification = result.Result.ShouldSucceed();
            return new ThenSuccessBuilder<TSuccessResponse, TSuccessVerification>(this, successVerification);
        }

        public async Task<ThenFailureBuilder<TSuccessResponse, TSuccessVerification>> ShouldFail()
        {
            var result = await GetExecutionResult();
            return new ThenFailureBuilder<TSuccessResponse, TSuccessVerification>(this, result.Result);
        }

        public ThenOrderBuilder<TSuccessResponse, TSuccessVerification> Order(string orderNumber)
        {
            return new ThenOrderBuilder<TSuccessResponse, TSuccessVerification>(this, _app, orderNumber, _lazyExecute);
        }

        public async Task<ThenOrderBuilder<TSuccessResponse, TSuccessVerification>> Order()
        {
            var result = await GetExecutionResult();
            var orderNumber = result.OrderNumber;

            if (orderNumber == null)
            {
                throw new InvalidOperationException("Cannot verify order: no order number available from the executed operation");
            }

            return Order(orderNumber);
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> Coupon(string couponCode)
        {
            return new ThenCouponBuilder<TSuccessResponse, TSuccessVerification>(this, _app, couponCode);
        }

        public async Task<ThenCouponBuilder<TSuccessResponse, TSuccessVerification>> Coupon()
        {
            var result = await GetExecutionResult();
            var couponCode = result.CouponCode;

            if (couponCode == null)
            {
                throw new InvalidOperationException("Cannot verify coupon: no coupon code available from the executed operation");
            }

            return Coupon(couponCode);
        }

        internal async Task<ExecutionResult<TSuccessResponse, TSuccessVerification>> GetExecutionResult()
        {
            if (!_executionCompleted)
            {
                _executionResult = await _lazyExecute();
                _executionCompleted = true;
            }
            return _executionResult!;
        }
    }
}