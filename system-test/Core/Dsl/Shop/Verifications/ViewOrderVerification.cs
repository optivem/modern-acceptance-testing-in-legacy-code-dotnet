using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Verifications;

public class ViewOrderVerification : BaseSuccessVerification<GetOrderResponse>
{
    public ViewOrderVerification(GetOrderResponse response, TestContext context) 
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

    public ViewOrderVerification Status(OrderStatus expectedStatus)
    {
        Response.Status.ShouldBe(expectedStatus, 
            $"Expected status: {expectedStatus}, but got: {Response.Status}");
        return this;
    }

    public ViewOrderVerification Country(string expectedCountry)
    {
        Response.Country.ShouldBe(expectedCountry, 
            $"Expected country: '{expectedCountry}', but got: '{Response.Country}'");
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
