using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

#if false // Entire test file disabled
public class PublishCouponPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPublishCouponWithValidData(Channel channel)
    {
        await Scenario(channel)
            .When().PublishCoupon().WithCouponCode("SUMMER2025").WithDiscountRate("0.15")
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldPublishCouponWithOptionalFields(Channel channel)
    {
        var couponBuilder = await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("WINTER2025")
                .WithDiscountRate("0.20")
                .WithValidFrom("2025-01-01")
                .WithValidTo("2025-03-31")
                .WithUsageLimit(100)
            .Then().Coupon();
        
        await couponBuilder.HasDiscountRate(0.20m);
        await couponBuilder.HasUsageLimit(100);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("0.05")]
    [ChannelInlineData("0.10")]
    [ChannelInlineData("0.15")]
    [ChannelInlineData("0.25")]
    public async Task ShouldPublishCouponWithVariousDiscountRates(Channel channel, string discountRate)
    {
        var couponBuilder = await Scenario(channel)
            .When().PublishCoupon().WithDiscountRate(discountRate)
            .Then().Coupon();
        
        await couponBuilder.HasDiscountRate(decimal.Parse(discountRate));
    }
}
#endif
