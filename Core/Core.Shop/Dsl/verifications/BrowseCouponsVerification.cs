using Commons.Dsl;
using Shouldly;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

public class BrowseCouponsVerification : ResponseVerification<BrowseCouponsResponse>
{
    public BrowseCouponsVerification(BrowseCouponsResponse response, UseCaseContext context)
        : base(response, context)
    {
    }

    public BrowseCouponsVerification HasCouponWithCode(string? couponCodeAlias)
    {
        FindCouponByCode(couponCodeAlias); // Throws if not found
        return this;
    }

    public BrowseCouponsVerification CouponHasDiscountRate(string? couponCodeAlias, decimal expectedDiscountRate)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        coupon.DiscountRate.ShouldBe(expectedDiscountRate, $"Expected coupon '{couponCodeAlias}' to have discount rate {expectedDiscountRate:F2}");
        return this;
    }

    public BrowseCouponsVerification CouponHasValidFrom(string? couponCodeAlias, string? expectedValidFrom)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        string? actualValidFromString = coupon.ValidFrom?.ToString() ?? null;
        actualValidFromString.ShouldBe(expectedValidFrom, $"Expected coupon '{couponCodeAlias}' to have validFrom '{expectedValidFrom}'");
        return this;
    }

    public BrowseCouponsVerification CouponHasUsageLimit(string? couponCodeAlias, int expectedUsageLimit)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        coupon.UsageLimit.ShouldNotBeNull($"Expected coupon '{couponCodeAlias}' to have a usage limit");
        coupon.UsageLimit.ShouldBe(expectedUsageLimit, $"Expected coupon '{couponCodeAlias}' to have usage limit {expectedUsageLimit}");
        return this;
    }

    public BrowseCouponsVerification CouponHasUsedCount(string? couponCode, int expectedUsedCount)
    {
        var coupon = FindCouponByCode(couponCode);

        coupon.UsedCount.ShouldBe(expectedUsedCount, $"Expected coupon '{couponCode}' to have used count {expectedUsedCount}");
        return this;
    }

    private CouponDto FindCouponByCode(string? couponCodeAlias)
    {
        Response.ShouldNotBeNull();
        Response.Coupons.ShouldNotBeNull("No coupons found in response");

        var couponCode = Context.GetParamValue(couponCodeAlias);

        var coupon = Response.Coupons
            .Where(c => string.Equals(couponCode, c.Code))
            .FirstOrDefault();
        
        coupon.ShouldNotBeNull($"Coupon with code '{couponCode}' not found. Available coupons: [{string.Join(", ", Response.Coupons.Select(c => c.Code))}]");
        
        return coupon;
    }
}