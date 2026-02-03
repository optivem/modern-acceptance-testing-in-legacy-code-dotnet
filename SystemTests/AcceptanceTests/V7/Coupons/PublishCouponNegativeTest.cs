using Optivem.EShop.SystemTest.AcceptanceTests.Commons.Providers;
using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

#if false // Entire test file disabled
public class PublishCouponNegativeTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("-0.10")]  // Negative discount rate
    [ChannelInlineData("1.10")]   // Discount rate > 1
    public async Task ShouldRejectCouponWithInvalidDiscountRate(Channel channel, string invalidDiscountRate)
    {
        var then = Scenario(channel)
            .When().PublishCoupon().WithDiscountRate(invalidDiscountRate)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("discountRate", "Discount rate must be between 0 and 1");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelClassData(typeof(EmptyArgumentsProvider))]
    public async Task ShouldRejectCouponWithEmptyCouponCode(Channel channel, string emptyCouponCode)
    {
        var then = Scenario(channel)
            .When().PublishCoupon().WithCouponCode(emptyCouponCode).WithDiscountRate("0.15")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.API)]
    public async Task ShouldRejectCouponWithNullCouponCode(Channel channel)
    {
        var then = Scenario(channel)
            .When().PublishCoupon().WithCouponCode(null).WithDiscountRate("0.15")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code must not be empty");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectDuplicateCouponCode(Channel channel)
    {
        var then = Scenario(channel)
            .Given().Coupon().WithCouponCode("DUPLICATE2025")
            .When().PublishCoupon().WithCouponCode("DUPLICATE2025").WithDiscountRate("0.15")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("Coupon code already exists: DUPLICATE2025");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData(-10)]
    [ChannelInlineData(0)]
    public async Task ShouldRejectCouponWithInvalidUsageLimit(Channel channel, int invalidUsageLimit)
    {
        var then = Scenario(channel)
            .When().PublishCoupon().WithUsageLimit(invalidUsageLimit)
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("usageLimit", "Usage limit must be positive");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldRejectCouponWithValidToBeforeValidFrom(Channel channel)
    {
        var then = Scenario(channel)
            .When().PublishCoupon()
                .WithValidFrom("2025-03-31")
                .WithValidTo("2025-01-01")
            .Then();

        (await then.ShouldFail())
            .ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("validTo", "Valid to date must be after valid from date");
    }
}
#endif
