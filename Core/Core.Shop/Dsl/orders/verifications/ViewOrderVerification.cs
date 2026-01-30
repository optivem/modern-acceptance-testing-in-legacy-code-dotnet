using Commons.Dsl;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;
using Shouldly;
using System.Globalization;
using Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

public class ViewOrderVerification : ResponseVerification<ViewOrderResponse>
{
    public ViewOrderVerification(ViewOrderResponse response, UseCaseContext context) 
        : base(response, context)
    {
    }

    public ViewOrderVerification OrderNumber(string orderNumberResultAlias)
    {
        var expectedOrderNumber = Context.GetResultValue(orderNumberResultAlias);
        Response.OrderNumber.ShouldBe(expectedOrderNumber, 
            $"Expected order number to be '{expectedOrderNumber}', but was '{Response.OrderNumber}'");
        return this;
    }

    public ViewOrderVerification Sku(string skuParamAlias)
    {
        var expectedSku = Context.GetParamValue(skuParamAlias);
        Response.Sku.ShouldBe(expectedSku, 
            $"Expected SKU to be '{expectedSku}', but was '{Response.Sku}'");
        return this;
    }

    public ViewOrderVerification Quantity(int expectedQuantity)
    {
        Response.Quantity.ShouldBe(expectedQuantity, 
            $"Expected quantity: {expectedQuantity}, but got: {Response.Quantity}");
        return this;
    }

    public ViewOrderVerification Quantity(string expectedQuantity)
    {
        return Quantity(Converter.ToInteger(expectedQuantity).Value);
    }

    public ViewOrderVerification Status(OrderStatus expectedStatus)
    {
        Response.Status.ShouldBe(expectedStatus, 
            $"Expected status: {expectedStatus}, but got: {Response.Status}");
        return this;
    }

    public ViewOrderVerification Status(string expectedStatus)
    {
        return Status(Enum.Parse<OrderStatus>(expectedStatus));
    }

    public ViewOrderVerification Country(string expectedCountry)
    {
        Response.Country.ShouldBe(expectedCountry, 
            $"Expected country: '{expectedCountry}', but got: '{Response.Country}'");
        return this;
    }

    public ViewOrderVerification UnitPrice(decimal expectedUnitPrice)
    {
        Response.UnitPrice.ShouldBe(expectedUnitPrice,
            $"Expected unit price: {expectedUnitPrice}, but got: {Response.UnitPrice}");
        return this;
    }

    public ViewOrderVerification UnitPrice(string expectedUnitPrice)
    {
        return UnitPrice(Converter.ToBigDecimal(expectedUnitPrice)!.Value);
    }

    public ViewOrderVerification UnitPriceGreaterThanZero()
    {
        Response.UnitPrice.ShouldBeGreaterThan(0m, 
            $"Unit price should be positive, but was: {Response.UnitPrice}");
        return this;
    }

    public ViewOrderVerification SubtotalPrice(decimal expectedSubtotalPrice)
    {
        Response.SubtotalPrice.ShouldBe(expectedSubtotalPrice,
            $"Expected subtotal price: {expectedSubtotalPrice}, but got: {Response.SubtotalPrice}");
        return this;
    }

    public ViewOrderVerification SubtotalPrice(string expectedSubtotalPrice)
    {
        return SubtotalPrice(Converter.ToBigDecimal(expectedSubtotalPrice)!.Value);
    }

    public ViewOrderVerification SubtotalPriceGreaterThanZero()
    {
        Response.SubtotalPrice.ShouldBeGreaterThan(0m, 
            $"Subtotal price should be positive, but was: {Response.SubtotalPrice}");
        return this;
    }

    public ViewOrderVerification DiscountRateGreaterThanOrEqualToZero()
    {
        Response.DiscountRate.ShouldBeGreaterThanOrEqualTo(0m, 
            $"Discount rate should be non-negative, but was: {Response.DiscountRate}");
        return this;
    }

    public ViewOrderVerification DiscountAmountGreaterThanOrEqualToZero()
    {
        Response.DiscountAmount.ShouldBeGreaterThanOrEqualTo(0m, 
            $"Discount amount should be non-negative, but was: {Response.DiscountAmount}");
        return this;
    }

    public ViewOrderVerification TaxRateGreaterThanOrEqualToZero()
    {
        Response.TaxRate.ShouldBeGreaterThanOrEqualTo(0m, 
            $"Tax rate should be non-negative, but was: {Response.TaxRate}");
        return this;
    }

    public ViewOrderVerification TaxAmountGreaterThanOrEqualToZero()
    {
        Response.TaxAmount.ShouldBeGreaterThanOrEqualTo(0m, 
            $"Tax amount should be non-negative, but was: {Response.TaxAmount}");
        return this;
    }

    public ViewOrderVerification TotalPriceGreaterThanZero()
    {
        Response.TotalPrice.ShouldBeGreaterThan(0m, 
            $"Total price should be positive, but was: {Response.TotalPrice}");
        return this;
    }

    // Base Price Methods
    public ViewOrderVerification BasePrice(decimal expectedBasePrice)
    {
        Response.BasePrice.ShouldBe(expectedBasePrice, 
            $"Expected base price to be {expectedBasePrice}, but was {Response.BasePrice}");
        return this;
    }

    public ViewOrderVerification BasePrice(string expectedBasePrice)
    {
        return BasePrice(Converter.ToBigDecimal(expectedBasePrice)!.Value);
    }

    public ViewOrderVerification BasePriceGreaterThanZero()
    {
        Response.BasePrice.ShouldBeGreaterThan(0m, 
            $"Base price should be positive, but was: {Response.BasePrice}");
        return this;
    }

    // Discount Rate Methods
    public ViewOrderVerification DiscountRate(decimal expectedDiscountRate)
    {
        Response.DiscountRate.ShouldBe(expectedDiscountRate, 
            $"Expected discount rate to be {expectedDiscountRate}, but was {Response.DiscountRate}");
        return this;
    }

    public ViewOrderVerification DiscountRate(string expectedDiscountRate)
    {
        return DiscountRate(Converter.ToBigDecimal(expectedDiscountRate)!.Value);
    }

    // Discount Amount Methods  
    public ViewOrderVerification DiscountAmount(decimal expectedDiscountAmount)
    {
        Response.DiscountAmount.ShouldBe(expectedDiscountAmount, 
            $"Expected discount amount to be {expectedDiscountAmount}, but was {Response.DiscountAmount}");
        return this;
    }

    public ViewOrderVerification DiscountAmount(string expectedDiscountAmount)
    {
        return DiscountAmount(Converter.ToBigDecimal(expectedDiscountAmount)!.Value);
    }

    // Tax Rate Methods
    public ViewOrderVerification TaxRate(decimal expectedTaxRate)
    {
        Response.TaxRate.ShouldBe(expectedTaxRate, 
            $"Expected tax rate to be {expectedTaxRate}, but was {Response.TaxRate}");
        return this;
    }

    public ViewOrderVerification TaxRate(string expectedTaxRate)
    {
        return TaxRate(Converter.ToBigDecimal(expectedTaxRate)!.Value);
    }

    // Tax Amount Methods
    public ViewOrderVerification TaxAmount(decimal expectedTaxAmount)
    {
        Response.TaxAmount.ShouldBe(expectedTaxAmount, 
            $"Expected tax amount to be {expectedTaxAmount}, but was {Response.TaxAmount}");
        return this;
    }

    public ViewOrderVerification TaxAmount(string expectedTaxAmount)
    {
        return TaxAmount(Converter.ToBigDecimal(expectedTaxAmount)!.Value);
    }

    // Total Price Methods
    public ViewOrderVerification TotalPrice(decimal expectedTotalPrice)
    {
        Response.TotalPrice.ShouldBe(expectedTotalPrice, 
            $"Expected total price to be {expectedTotalPrice}, but was {Response.TotalPrice}");
        return this;
    }

    public ViewOrderVerification TotalPrice(string expectedTotalPrice)
    {
        return TotalPrice(Converter.ToBigDecimal(expectedTotalPrice)!.Value);
    }

    // Applied Coupon Code Methods
    public ViewOrderVerification AppliedCouponCode(string expectedCouponCodeAlias)
    {
        var expectedCouponCode = Context.GetParamValue(expectedCouponCodeAlias);
        Response.AppliedCouponCode.ShouldBe(expectedCouponCode, 
            $"Expected applied coupon code to be '{expectedCouponCode}', but was '{Response.AppliedCouponCode}'");
        return this;
    }

    public ViewOrderVerification AppliedCouponCodeIsNull()
    {
        Response.AppliedCouponCode.ShouldBeNull(
            $"Expected applied coupon code to be null, but was '{Response.AppliedCouponCode}'");
        return this;
    }

    // Order Number Prefix Method
    public ViewOrderVerification OrderNumberHasPrefix(string expectedPrefix)
    {
        Response.OrderNumber.ShouldStartWith(expectedPrefix, Case.Sensitive,
            $"Expected order number to start with '{expectedPrefix}', but was: {Response.OrderNumber}");
        return this;
    }

    // Order Timestamp Method
    public ViewOrderVerification OrderTimestampIsNotNull()
    {
        Response.OrderTimestamp.ShouldNotBe(default, "Expected order timestamp to be set, but was null");
        return this;
    }
}
