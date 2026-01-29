using Dsl.Gherkin.Then;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

namespace Dsl.Gherkin.Then
{
    public class ThenCouponBuilder<TSuccessResponse, TSuccessVerification> 
        : BaseThenBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly SystemDsl _app;
        private readonly string _couponCode;
        private BrowseCouponsVerification? _verification;

        public ThenCouponBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause, SystemDsl app, string couponCode) : base(thenClause)
        {
            _app = app;
            _couponCode = couponCode;
        }

        private async Task<BrowseCouponsVerification> GetVerification()
        {
            if (_verification == null)
            {
                var result = await _app.Shop(Channel).BrowseCoupons().Execute();
                _verification = result.ShouldSucceed();
                _verification.HasCouponWithCode(_couponCode);
            }
            return _verification;
        }

        public async Task<ThenCouponBuilder<TSuccessResponse, TSuccessVerification>> HasDiscountRate(decimal discountRate)
        {
            var verification = await GetVerification();
            verification.CouponHasDiscountRate(_couponCode, discountRate);
            return this;
        }

        public async Task<ThenCouponBuilder<TSuccessResponse, TSuccessVerification>> IsValidFrom(string validFrom)
        {
            var verification = await GetVerification();
            verification.CouponHasValidFrom(_couponCode, validFrom);
            return this;
        }

        public async Task<ThenCouponBuilder<TSuccessResponse, TSuccessVerification>> HasUsageLimit(int usageLimit)
        {
            var verification = await GetVerification();
            verification.CouponHasUsageLimit(_couponCode, usageLimit);
            return this;
        }

        public async Task<ThenCouponBuilder<TSuccessResponse, TSuccessVerification>> HasUsedCount(int expectedUsedCount)
        {
            var verification = await GetVerification();
            verification.CouponHasUsedCount(_couponCode, expectedUsedCount);
            return this;
        }
    }
}