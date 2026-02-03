using Optivem.EShop.SystemTest.AcceptanceTests.V7.Base;
using Optivem.EShop.SystemTest.Core.Shop;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.Testing;
using Xunit;

namespace Optivem.EShop.SystemTest.AcceptanceTests.V7.Orders;

public class PlaceOrderPositiveTest : BaseAcceptanceTest
{
    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldBeAbleToPlaceOrderForValidInput(Channel channel)
    {
        await Scenario(channel)
            .Given().Product().WithSku("ABC").WithUnitPrice(20.00)
            .And().Country().WithCode("US").WithTaxRate(0.10)
            .When().PlaceOrder().WithSku("ABC").WithQuantity(5).WithCountry("US")
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task OrderStatusShouldBePlacedAfterPlacingOrder(Channel channel)
    {
        await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed()
            .And().Order().HasStatus(OrderStatus.Placed);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldCalculateBasePriceAsProductOfUnitPriceAndQuantity(Channel channel)
    {
        await Scenario(channel)
            .Given().Product().WithUnitPrice(20.00)
            .When().PlaceOrder().WithQuantity(5)
            .Then().Order().HasBasePrice(100.00);
    }

    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, 20.00, 5, 100.00)]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, 10.00, 3, 30.00)]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, 15.50, 4, 62.00)]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, 99.99, 1, 99.99)]
    public async Task ShouldPlaceOrderWithCorrectBasePriceParameterized(Channel channel, double unitPrice, int quantity, double basePrice)
    {
        await Scenario(channel)
            .Given().Product().WithUnitPrice(unitPrice)
            .When().PlaceOrder().WithQuantity(quantity)
            .Then().Order().HasBasePrice(basePrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task OrderPrefixShouldBeORD(Channel channel)
    {
        await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed().HasOrderNumberPrefix("ORD-")
            .And().Order().HasOrderNumberPrefix("ORD-");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task DiscountRateShouldBeAppliedForCoupon(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("SUMMER2025").WithDiscountRate(0.15)
            .When().PlaceOrder().WithCouponCode("SUMMER2025")
            .Then().Order().HasAppliedCoupon("SUMMER2025")
            .HasDiscountRate(0.15);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task DiscountRateShouldBeNotAppliedWhenThereIsNoCoupon(Channel channel)
    {
        await Scenario(channel)
            .When().PlaceOrder().WithCouponCode(null)
            .Then().Order().HasStatus(OrderStatus.Placed)
            .HasAppliedCoupon(null)
            .HasDiscountRate(0.00)
            .HasDiscountAmount(0.00);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task SubtotalPriceShouldBeCalculatedAsTheBasePriceMinusDiscountAmountWhenWeHaveCoupon(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithDiscountRate(0.15)
            .And().Product().WithUnitPrice(20.00)
            .When().PlaceOrder().WithCouponCode().WithQuantity(5)
            .Then().Order().HasAppliedCoupon()
            .HasDiscountRate(0.15)
            .HasBasePrice(100.00)
            .HasDiscountAmount(15.00)
            .HasSubtotalPrice(85.00);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task SubtotalPriceShouldBeSameAsBasePriceWhenNoCoupon(Channel channel)
    {
        await Scenario(channel)
            .Given().Product().WithUnitPrice(20.00)
            .When().PlaceOrder().WithQuantity(5)
            .Then().Order()
            .HasBasePrice(100.00)
            .HasDiscountAmount(0.00)
            .HasSubtotalPrice(100.00);
    }

    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "UK", 0.09)]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "US", 0.20)]
    public async Task CorrectTaxRateShouldBeUsedBasedOnCountry(Channel channel, string country, double taxRate)
    {
        await Scenario(channel)
            .Given().Country().WithCode(country).WithTaxRate(taxRate)
            .When().PlaceOrder().WithCountry(country)
            .Then().Order().HasTaxRate(taxRate);
    }

    [Theory]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "UK", 0.09, 50.00, 4.50, 54.50)]
    [ChannelInlineData(ChannelType.UI, ChannelType.API, "US", 0.20, 100.00, 20.00, 120.00)]
    public async Task TotalPriceShouldBeSubtotalPricePlusTaxAmount(Channel channel, string country, double taxRate, double subtotalPrice, double expectedTaxAmount, double expectedTotalPrice)
    {
        await Scenario(channel)
            .Given().Country().WithCode(country).WithTaxRate(taxRate)
            .And().Product().WithUnitPrice(subtotalPrice)
            .When().PlaceOrder().WithCountry(country).WithQuantity(1)
            .Then().ShouldSucceed()
            .And().Order().HasTaxRate(taxRate)
            .HasSubtotalPrice(subtotalPrice)
            .HasTaxAmount(expectedTaxAmount)
            .HasTotalPrice(expectedTotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CouponUsageCountHasBeenIncrementedAfterItsBeenUsed(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("SUMMER2025")
            .When().PlaceOrder().WithCouponCode("SUMMER2025")
            .Then().Coupon("SUMMER2025").HasUsedCount(1);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPlaceOrderWithNonExistentCoupon(Channel channel)
    {
        await Scenario(channel)
            .When().PlaceOrder().WithCouponCode("INVALIDCOUPON")
            .Then().ShouldFail().ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code INVALIDCOUPON does not exist");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPlaceOrderWithExpiredCoupon(Channel channel)
    {
        await Scenario(channel)
            .Given().Clock().WithTime("2023-09-01T12:00:00Z")
            .And().Coupon().WithCouponCode("SUMMER2023").WithValidFrom("2023-06-01T00:00:00Z").WithValidTo("2023-08-31T23:59:59Z")
            .When().PlaceOrder().WithCouponCode("SUMMER2023")
            .Then().ShouldFail().ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code SUMMER2023 has expired");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPlaceOrderWithCouponThatHasExceededUsageLimit(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("LIMITED2024").WithUsageLimit(2)
            .And().Order().WithOrderNumber("ORD-1").WithCouponCode("LIMITED2024")
            .And().Order().WithOrderNumber("ORD-2").WithCouponCode("LIMITED2024")
            .When().PlaceOrder().WithOrderNumber("ORD-3").WithCouponCode("LIMITED2024")
            .Then().ShouldFail().ErrorMessage("The request contains one or more validation errors")
            .FieldErrorMessage("couponCode", "Coupon code LIMITED2024 has exceeded its usage limit");
    }
}
