using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

public class PublishCouponPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToPublishValidCoupon(Channel channel)
    {
        await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("SUMMER2025")
                .WithDiscountRate(0.15m)
                .WithValidFrom("2024-06-01T00:00:00Z")
                .WithValidTo("2024-08-31T23:59:59Z")
                .WithUsageLimit(100)
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToPublishCouponWithEmptyOptionalFields(Channel channel)
    {
        await Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("SUMMER2025")
                .WithDiscountRate(0.15m)
                .WithValidFrom("")
                .WithValidTo("")
                .WithUsageLimit("")
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToCorrectlySaveCoupon(Channel channel)
    {
        var couponBuilder = Scenario(channel)
            .When().PublishCoupon()
                .WithCouponCode("SUMMER2025")
                .WithDiscountRate(0.15m)
                .WithValidFrom("2024-06-01T00:00:00Z")
                .WithValidTo("2024-08-31T23:59:59Z")
                .WithUsageLimit(100)
            .Then().Coupon("SUMMER2025");

        await couponBuilder.HasDiscountRate(0.15m);
        await couponBuilder.IsValidFrom("2024-06-01T00:00:00Z");
        await couponBuilder.HasUsageLimit(100);
        await couponBuilder.HasUsedCount(0);
    }
}