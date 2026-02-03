using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.Testing;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Coupons;

#if false // Entire test file disabled
public class BrowseCouponsPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToBrowseCoupons(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("SUMMER2025").WithDiscountRate("0.15")
            .And().Coupon().WithCouponCode("WINTER2025").WithDiscountRate("0.20")
            .When().BrowseCoupons()
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldReturnEmptyListWhenNoCoupons(Channel channel)
    {
        await Scenario(channel)
            .When().BrowseCoupons()
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBrowseCouponsWithFiltering(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("ACTIVE2025").WithDiscountRate("0.15").WithValidFrom("2025-01-01").WithValidTo("2025-12-31")
            .And().Coupon().WithCouponCode("EXPIRED2024").WithDiscountRate("0.10").WithValidFrom("2024-01-01").WithValidTo("2024-12-31")
            .When().BrowseCoupons()
            .Then().Coupon("ACTIVE2025").HasDiscountRate(0.15m);
    }
}
#endif
