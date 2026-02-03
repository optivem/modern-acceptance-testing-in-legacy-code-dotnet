using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

public class PublishCouponNegativeTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "0.0")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "-0.01")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "-0.15")]
    public async Task CannotPublishCouponWithZeroOrNegativeDiscount(Channel channel, string discountRate)
    {
        await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-COUPON")
                .WithDiscountRate(discountRate)
            .Then().ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("discountRate", "Discount rate must be greater than 0.00");
    }

    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "1.01")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "2.00")]
    public async Task CannotPublishCouponWithDiscountGreaterThan100Percent(Channel channel, string discountRate)
    {
        await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-COUPON")
                .WithDiscountRate(discountRate)
            .Then().ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("discountRate", "Discount rate must be at most 1.00");
    }

    [Theory(Skip = "Disabled until bug is fixed")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "2023-12-31T23:59:59Z")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "2024-01-01T00:00:00Z")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "2025-06-01T12:00:00Z")]
    public async Task CannotPublishCouponWithValidToInThePast(Channel channel, string validTo)
    {
        await Scenario(channel)
            .Given().Clock().WithTime("2026-01-01T12:00:00Z")
            .When().PublishCoupon()
                .WithCouponCode("PAST-COUPON")
                .WithDiscountRate(0.15)
                .WithValidTo(validTo)
            .Then().ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("validTo", "Valid to date cannot be in the past");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPublishCouponWithDuplicateCouponCode(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon()
                .WithCouponCode("EXISTING-COUPON")
                .WithDiscountRate(0.10)
            .When().PublishCoupon()
                .WithCouponCode("EXISTING-COUPON")
                .WithDiscountRate(0.20)
            .Then().ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("couponCode", "Coupon code EXISTING-COUPON already exists");
    }


    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "0")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "-1")]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "-100")]
    public async Task CannotPublishCouponWithZeroOrNegativeUsageLimit(Channel channel, string usageLimit)
    {
        await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-LIMIT")
                .WithDiscountRate(0.15)
                .WithUsageLimit(usageLimit)
            .Then().ShouldFail()
                .ErrorMessage("The request contains one or more validation errors")
                .FieldErrorMessage("usageLimit", "Usage limit must be positive");
    }
}
