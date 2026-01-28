using Dsl.Gherkin.Then;
using Optivem.Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

namespace Dsl.Gherkin.Then
{
    public class ThenCouponBuilder<TSuccessResponse, TSuccessVerification> 
        : BaseThenBuilder<TSuccessResponse, TSuccessVerification>
        where TSuccessVerification : ResponseVerification<TSuccessResponse>
    {
        private readonly BrowseCouponsVerification _verification;
        private readonly string _couponCode;

        public ThenCouponBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause, SystemDsl app, string couponCode) : base(thenClause)
        {
            _couponCode = couponCode;
            _verification = app.Shop(Channel).BrowseCoupons()
                .Execute()
                .ShouldSucceed();

            _verification.HasCouponWithCode(couponCode);
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> HasDiscountRate(double discountRate)
        {
            _verification.CouponHasDiscountRate(_couponCode, discountRate);
            return this;
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> IsValidFrom(string validFrom)
        {
            _verification.CouponHasValidFrom(_couponCode, validFrom);
            return this;
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> HasUsageLimit(int usageLimit)
        {
            _verification.CouponHasUsageLimit(_couponCode, usageLimit);
            return this;
        }

        public ThenCouponBuilder<TSuccessResponse, TSuccessVerification> HasUsedCount(int expectedUsedCount)
        {
            _verification.CouponHasUsedCount(_couponCode, expectedUsedCount);
            return this;
        }
    }
}