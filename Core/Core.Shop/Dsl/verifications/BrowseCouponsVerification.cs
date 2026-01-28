using Optivem.Commons.Dsl;
using Shouldly;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Coupons;
using System;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

public class BrowseCouponsVerification : ResponseVerification<BrowseCouponsResponse>
{
    public BrowseCouponsVerification(BrowseCouponsResponse response, UseCaseContext context)
        : base(response, context)
    {
    }

    public BrowseCouponsVerification HasCouponWithCode(string couponCodeAlias)
    {
        FindCouponByCode(couponCodeAlias); // Throws if not found
        return this;
    }

    public BrowseCouponsVerification CouponHasDiscountRate(string couponCodeAlias, double expectedDiscountRate)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        var actualDiscountRate = coupon.DiscountRate;
        if (Math.Abs(actualDiscountRate - expectedDiscountRate) > 0.0001)
        {
            throw new ShouldAssertException($"Expected coupon '{couponCodeAlias}' to have discount rate {expectedDiscountRate:F2}, but was {actualDiscountRate:F2}");
        }
        return this;
    }

    public BrowseCouponsVerification CouponHasValidFrom(string couponCodeAlias, string expectedValidFrom)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        var actualValidFrom = coupon.ValidFrom;
        var actualValidFromString = actualValidFrom?.ToString("yyyy-MM-ddTHH:mm:ssZ");
        if (!expectedValidFrom.Equals(actualValidFromString))
        {
            throw new ShouldAssertException($"Expected coupon '{couponCodeAlias}' to have validFrom '{expectedValidFrom}', but was '{actualValidFromString}'");
        }
        return this;
    }

    public BrowseCouponsVerification CouponHasUsageLimit(string couponCodeAlias, int expectedUsageLimit)
    {
        var coupon = FindCouponByCode(couponCodeAlias);

        var actualUsageLimit = coupon.UsageLimit;
        if (actualUsageLimit == null || actualUsageLimit != expectedUsageLimit)
        {
            throw new ShouldAssertException($"Expected coupon '{couponCodeAlias}' to have usage limit {expectedUsageLimit}, but was {actualUsageLimit}");
        }
        return this;
    }

    public BrowseCouponsVerification CouponHasUsedCount(string couponCode, int expectedUsedCount)
    {
        var coupon = FindCouponByCode(couponCode);

        var actualUsedCount = coupon.UsedCount;
        if (actualUsedCount != expectedUsedCount)
        {
            throw new ShouldAssertException($"Expected coupon '{couponCode}' to have used count {expectedUsedCount}, but was {actualUsedCount}");
        }
        return this;
    }

    public BrowseCouponsVerification ShouldHaveCoupons()
    {
        Response.Coupons.ShouldNotBeNull();
        Response.Coupons.ShouldNotBeEmpty();
        return this;
    }

    public BrowseCouponsVerification ShouldHaveCouponsCount(int expectedCount)
    {
        Response.Coupons.ShouldNotBeNull();
        Response.Coupons.Count.ShouldBe(expectedCount);
        return this;
    }

    public BrowseCouponsVerification ShouldContainCoupon(string couponCode)
    {
        Response.Coupons.ShouldNotBeNull();
        Response.Coupons.Any(c => c.Code == couponCode).ShouldBeTrue();
        return this;
    }

    private CouponDto FindCouponByCode(string couponCodeAlias)
    {
        if (Response?.Coupons == null)
        {
            throw new ShouldAssertException("No coupons found in response");
        }

        var couponCode = Context.GetParamValue(couponCodeAlias);

        return Response.Coupons
            .FirstOrDefault(c => couponCode.Equals(c.Code))
            ?? throw new ShouldAssertException($"Coupon with code '{couponCode}' not found. Available coupons: [{string.Join(", ", Response.Coupons.Select(c => c.Code))}]");
    }
}