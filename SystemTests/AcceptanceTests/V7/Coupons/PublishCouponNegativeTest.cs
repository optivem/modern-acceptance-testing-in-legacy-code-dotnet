using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

public class PublishCouponNegativeTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("0.0")]
    [ChannelInlineData("-0.01")]
    [ChannelInlineData("-0.15")]
    public async Task CannotPublishCouponWithZeroOrNegativeDiscount(Channel channel, string discountRate)
    {
        var then = Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-COUPON")
                .WithDiscountRate(discountRate)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("discountRate", "Discount rate must be greater than 0.00");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("1.01")]
    [ChannelInlineData("2.00")]
    public async Task CannotPublishCouponWithDiscountGreaterThan100Percent(Channel channel, string discountRate)
    {
        var then = Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-COUPON")
                .WithDiscountRate(discountRate)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("discountRate", "Discount rate must be at most 1.00");
    }

    [Theory]
    // [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelData(ChannelType.API)]
    public async Task CannotPublishCouponWithDuplicateCouponCode(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Coupon()
                .WithCouponCode("EXISTING-COUPON")
                .WithDiscountRate(0.10m)
            .When().PublishCoupon()
                .WithCouponCode("EXISTING-COUPON")
                .WithDiscountRate(0.20m)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code EXISTING-COUPON already exists");
    }


    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("0")]
    [ChannelInlineData("-1")]
    [ChannelInlineData("-100")]
    public async Task CannotPublishCouponWithZeroOrNegativeUsageLimit(Channel channel, string usageLimit)
    {
        var then = Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("INVALID-LIMIT")
                .WithDiscountRate(0.15m)
                .WithUsageLimit(usageLimit)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("usageLimit", "Usage limit must be positive");
    }
}