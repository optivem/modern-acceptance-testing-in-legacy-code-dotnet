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
            .Given().Product().WithSku("ABC").WithUnitPrice(20.00m)
            .And().Country().WithCode("US").WithTaxRate(0.10m)
            .When().PlaceOrder().WithSku("ABC").WithQuantity(5).WithCountry("US")
            .Then().ShouldSucceed();
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task OrderStatusShouldBePlacedAfterPlacingOrder(Channel channel)
    {
        var successBuilder = await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed();
        
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasStatus(OrderStatus.Placed);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task ShouldCalculateBasePriceAsProductOfUnitPriceAndQuantity(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Product().WithUnitPrice(20.00m)
            .When().PlaceOrder().WithQuantity(5)
            .Then().Order();
        
        await orderBuilder.HasBasePrice(100.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("20.00", 5, "100.00")]
    [ChannelInlineData("10.00", 3, "30.00")]
    [ChannelInlineData("15.50", 4, "62.00")]
    [ChannelInlineData("99.99", 1, "99.99")]
    public async Task ShouldPlaceOrderWithCorrectBasePriceParameterized(Channel channel, string unitPrice, int quantity, string basePrice)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Product().WithUnitPrice(unitPrice)
            .When().PlaceOrder().WithQuantity(quantity)
            .Then().Order();
        
        await orderBuilder.HasBasePrice(basePrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task OrderPrefixShouldBeORD(Channel channel)
    {
        var successBuilder = await Scenario(channel)
            .When().PlaceOrder()
            .Then().ShouldSucceed();
        
        successBuilder.HasOrderNumberPrefix("ORD-");
        
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasOrderNumberPrefix("ORD-");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task DiscountRateShouldBeAppliedForCoupon(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Coupon().WithCouponCode("SUMMER2025").WithDiscountRate(0.15m)
            .When().PlaceOrder().WithCouponCode("SUMMER2025")
            .Then().Order();
        
        await orderBuilder.HasAppliedCoupon("SUMMER2025");
        await orderBuilder.HasDiscountRate(0.15m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task DiscountRateShouldBeNotAppliedWhenThereIsNoCoupon(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .When().PlaceOrder().WithCouponCode(null)
            .Then().Order();
        
        await orderBuilder.HasStatus(OrderStatus.Placed);
        await orderBuilder.HasAppliedCoupon(null!);
        await orderBuilder.HasDiscountRate(0.00m);
        await orderBuilder.HasDiscountAmount(0.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task SubtotalPriceShouldBeCalculatedAsTheBasePriceMinusDiscountAmountWhenWeHaveCoupon(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Coupon().WithDiscountRate(0.15m)
            .And().Product().WithUnitPrice(20.00m)
            .When().PlaceOrder().WithCouponCode().WithQuantity(5)
            .Then().Order();
        
        await orderBuilder.HasAppliedCoupon();
        await orderBuilder.HasDiscountRate(0.15m);
        await orderBuilder.HasBasePrice(100.00m);
        await orderBuilder.HasDiscountAmount(15.00m);
        await orderBuilder.HasSubtotalPrice(85.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task SubtotalPriceShouldBeSameAsBasePriceWhenNoCoupon(Channel channel)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Product().WithUnitPrice(20.00m)
            .When().PlaceOrder().WithQuantity(5)
            .Then().Order();
        
        await orderBuilder.HasBasePrice(100.00m);
        await orderBuilder.HasDiscountAmount(0.00m);
        await orderBuilder.HasSubtotalPrice(100.00m);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("UK", 0.09)]
    [ChannelInlineData("US", 0.20)]
    public async Task CorrectTaxRateShouldBeUsedBasedOnCountry(Channel channel, string country, double taxRate)
    {
        var orderBuilder = await Scenario(channel)
            .Given().Country().WithCode(country).WithTaxRate((decimal)taxRate)
            .When().PlaceOrder().WithCountry(country)
            .Then().Order();
        
        await orderBuilder.HasTaxRate((decimal)taxRate);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelInlineData("UK", 0.09, "50.00", "4.50", "54.50")]
    [ChannelInlineData("US", 0.20, "100.00", "20.00", "120.00")]
    public async Task TotalPriceShouldBeSubtotalPricePlusTaxAmount(Channel channel, string country, double taxRate, string subtotalPrice, string expectedTaxAmount, string expectedTotalPrice)
    {
        var successBuilder = await Scenario(channel)
            .Given().Country().WithCode(country).WithTaxRate((decimal)taxRate)
            .And().Product().WithUnitPrice(subtotalPrice)
            .When().PlaceOrder().WithCountry(country).WithQuantity(1)
            .Then().ShouldSucceed();
        
        var orderBuilder = await successBuilder.And().Order();
        await orderBuilder.HasTaxRate((decimal)taxRate);
        await orderBuilder.HasSubtotalPrice(subtotalPrice);
        await orderBuilder.HasTaxAmount(expectedTaxAmount);
        await orderBuilder.HasTotalPrice(expectedTotalPrice);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CouponUsageCountHasBeenIncrementedAfterItsBeenUsed(Channel channel)
    {
        await Scenario(channel)
            .Given().Coupon().WithCouponCode("SUMMER2025")
            .When().PlaceOrder().WithCouponCode("SUMMER2025")
            .Then().ShouldSucceed();

        var couponBuilder = Scenario(channel)
            .When().BrowseCoupons()
            .Then().Coupon("SUMMER2025");
        
        await couponBuilder.HasUsedCount(1);
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPlaceOrderWithNonExistentCoupon(Channel channel)
    {
        var failureBuilder = await Scenario(channel)
            .When().PlaceOrder().WithCouponCode("INVALIDCOUPON")
            .Then().ShouldFail();
        
        failureBuilder.ErrorMessage("The request contains one or more validation errors");
        failureBuilder.FieldErrorMessage("couponCode", "Coupon code INVALIDCOUPON does not exist");
    }

    [Theory]
    [ChannelData(ChannelType.UI, ChannelType.API)]
    public async Task CannotPlaceOrderWithExpiredCoupon(Channel channel)
    {
        var failureBuilder = await Scenario(channel)
            .Given().Clock().WithTime("2023-09-01T12:00:00Z")
            .And().Coupon().WithCouponCode("SUMMER2023").WithValidFrom("2023-06-01T00:00:00Z").WithValidTo("2023-08-31T23:59:59Z")
            .When().PlaceOrder().WithCouponCode("SUMMER2023")
            .Then().ShouldFail();
        
        failureBuilder.ErrorMessage("The request contains one or more validation errors");
        failureBuilder.FieldErrorMessage("couponCode", "Coupon code SUMMER2023 has expired");
    }

    [Theory]
    // [ChannelData(ChannelType.UI, ChannelType.API)]
    [ChannelData(ChannelType.UI)]
    public async Task CannotPlaceOrderWithCouponThatHasExceededUsageLimit(Channel channel)
    {
        var failureBuilder = await Scenario(channel)
            .Given().Coupon().WithCouponCode("LIMITED2024").WithUsageLimit(2)
            .And().Order().WithOrderNumber("ORD-1").WithCouponCode("LIMITED2024")
            .And().Order().WithOrderNumber("ORD-2").WithCouponCode("LIMITED2024")
            .When().PlaceOrder().WithOrderNumber("ORD-3").WithCouponCode("LIMITED2024")
            .Then().ShouldFail();
                 
        failureBuilder.ErrorMessage("The request contains one or more validation errors");
        failureBuilder.FieldErrorMessage("couponCode", "Coupon code LIMITED2024 has exceeded its usage limit");
    }
}
