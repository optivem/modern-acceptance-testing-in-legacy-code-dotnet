using System.Globalization;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.Testing.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

public class ViewOrderVerification : ResponseVerification<GetOrderResponse, UseCaseContext>
{
    public ViewOrderVerification(GetOrderResponse response, UseCaseContext context) 
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
        return Quantity(int.Parse(expectedQuantity));
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
        return UnitPrice(decimal.Parse(expectedUnitPrice, CultureInfo.InvariantCulture));
    }

    public ViewOrderVerification UnitPriceGreaterThanZero()
    {
        Response.UnitPrice.ShouldBeGreaterThan(0m, 
            $"Unit price should be positive, but was: {Response.UnitPrice}");
        return this;
    }

    public ViewOrderVerification OriginalPrice(decimal expectedOriginalPrice)
    {
        Response.OriginalPrice.ShouldBe(expectedOriginalPrice,
            $"Expected original price: {expectedOriginalPrice}, but got: {Response.OriginalPrice}");
        return this;
    }

    public ViewOrderVerification OriginalPrice(string expectedOriginalPrice)
    {
        return OriginalPrice(decimal.Parse(expectedOriginalPrice, CultureInfo.InvariantCulture));
    }

    public ViewOrderVerification OriginalPriceGreaterThanZero()
    {
        Response.OriginalPrice.ShouldBeGreaterThan(0m, 
            $"Original price should be positive, but was: {Response.OriginalPrice}");
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

    public ViewOrderVerification SubtotalPriceGreaterThanZero()
    {
        Response.SubtotalPrice.ShouldBeGreaterThan(0m, 
            $"Subtotal price should be positive, but was: {Response.SubtotalPrice}");
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
}
