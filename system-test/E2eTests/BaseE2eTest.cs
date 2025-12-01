using Shouldly;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Xunit;

namespace Optivem.EShop.SystemTest.E2eTests;

public abstract class BaseE2eTest : IDisposable
{
    protected IShopDriver ShopDriver;
    protected ErpApiDriver ErpApiDriver;
    protected TaxApiDriver TaxApiDriver;

    protected BaseE2eTest()
    {
        ShopDriver = CreateDriver();
        ErpApiDriver = DriverFactory.CreateErpApiDriver();
        TaxApiDriver = DriverFactory.CreateTaxApiDriver();
    }

    protected abstract IShopDriver CreateDriver();

    public void Dispose()
    {
        ShopDriver?.Dispose();
        ErpApiDriver?.Dispose();
        TaxApiDriver?.Dispose();
    }

    [Fact]
    public void ShouldPlaceOrderAndCalculateOriginalPrice()
    {
        var sku = "ABC-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "20.00").ShouldBeSuccess();

        var placeOrderResult = ShopDriver.PlaceOrder(sku, "5", "US").ShouldBeSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;

        orderNumber.ShouldStartWith("ORD-");

        var viewOrderResult = ShopDriver.ViewOrder(orderNumber!).ShouldBeSuccess();

        var viewOrderResponse = viewOrderResult.GetValue();
        viewOrderResponse.OrderNumber.ShouldBe(orderNumber);
        viewOrderResponse.Sku.ShouldBe(sku);
        viewOrderResponse.Quantity.ShouldBe(5);
        viewOrderResponse.Country.ShouldBe("US");
        viewOrderResponse.UnitPrice.ShouldBe(20.00m);
        viewOrderResponse.OriginalPrice.ShouldBe(100.00m);
        viewOrderResponse.Status.ShouldBe(OrderStatus.PLACED);

        var discountRate = viewOrderResponse.DiscountRate;
        var discountAmount = viewOrderResponse.DiscountAmount;
        var subtotalPrice = viewOrderResponse.SubtotalPrice;

        discountRate.ShouldBeGreaterThanOrEqualTo(0m);
        discountAmount.ShouldBeGreaterThanOrEqualTo(0m);
        subtotalPrice.ShouldBeGreaterThan(0m);

        var taxRate = viewOrderResponse.TaxRate;
        var taxAmount = viewOrderResponse.TaxAmount;
        var totalPrice = viewOrderResponse.TotalPrice;

        taxRate.ShouldBeGreaterThanOrEqualTo(0m, "Tax rate should be non-negative");
        taxAmount.ShouldBeGreaterThanOrEqualTo(0m, "Tax amount should be non-negative");
        totalPrice.ShouldBeGreaterThan(0m, "Total price should be positive");
    }

    [Fact]
    public void ShouldCancelOrder()
    {
        var sku = "XYZ-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "50.00").ShouldBeSuccess();

        var placeOrderResult = ShopDriver.PlaceOrder(sku, "2", "US").ShouldBeSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;
        ShopDriver.CancelOrder(orderNumber!).ShouldBeSuccess();

        var viewOrderResult = ShopDriver.ViewOrder(orderNumber!).ShouldBeSuccess();

        var viewOrderResponse = viewOrderResult.GetValue();
        viewOrderResponse.OrderNumber.ShouldBe(orderNumber);
        viewOrderResponse.Sku.ShouldBe(sku);
        viewOrderResponse.Quantity.ShouldBe(2);
        viewOrderResponse.Country.ShouldBe("US");
        viewOrderResponse.UnitPrice.ShouldBe(50.00m);
        viewOrderResponse.OriginalPrice.ShouldBe(100.00m);
        viewOrderResponse.Status.ShouldBe(OrderStatus.CANCELLED);
    }

    [Fact]
    public void ShouldRejectOrderWithNonExistentSku()
    {
        ShopDriver.PlaceOrder("NON-EXISTENT-SKU-12345", "5", "US")
            .ShouldBeFailure("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
    }

    [Fact]
    public void ShouldNotBeAbleToViewNonExistentOrder()
    {
        ShopDriver.ViewOrder("NON-EXISTENT-ORDER-12345")
            .ShouldBeFailure("Order NON-EXISTENT-ORDER-12345 does not exist.");
    }

    [Fact]
    public void ShouldRejectOrderWithNegativeQuantity()
    {
        var sku = "DEF-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "30.00").ShouldBeSuccess();

        ShopDriver.PlaceOrder(sku, "-3", "US")
            .ShouldBeFailure("Quantity must be positive");
    }

    [Fact]
    public void ShouldRejectOrderWithZeroQuantity()
    {
        var sku = "GHI-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "40.00").ShouldBeSuccess();

        ShopDriver.PlaceOrder(sku, "0", "US")
            .ShouldBeFailure("Quantity must be positive");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptySku(string sku)
    {
        ShopDriver.PlaceOrder(sku, "5", "US")
            .ShouldBeFailure("SKU must not be empty");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptyQuantity(string emptyQuantity)
    {
        ShopDriver.PlaceOrder("some-sku", emptyQuantity, "US")
            .ShouldBeFailure("Quantity must not be empty");
    }

    [Theory]
    [InlineData("3.5")]
    [InlineData("lala")]
    public void ShouldRejectOrderWithNonIntegerQuantity(string nonIntegerQuantity)
    {
        ShopDriver.PlaceOrder("some-sku", nonIntegerQuantity, "US")
            .ShouldBeFailure("Quantity must be an integer");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptyCountry(string emptyCountry)
    {
        ShopDriver.PlaceOrder("some-sku", "5", emptyCountry)
            .ShouldBeFailure("Country must not be empty");
    }

    public void ShouldRejectOrderWithUnsupportedCountry()
    {
        var sku = "JKL-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "25.00").ShouldBeSuccess();

        ShopDriver.PlaceOrder(sku, "3", "XX")
            .ShouldBeFailure("Country does not exist: XX");
    }
}
