using FluentAssertions;
using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Drivers.External.Tax.Api;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;
using Xunit;
using static Optivem.EShop.SystemTest.Core.Drivers.Commons.ResultAssert;

namespace Optivem.EShop.SystemTest.E2eTests;

public abstract class BaseE2eTest : IDisposable
{
    protected IShopDriver ShopDriver = null!;
    protected ErpApiDriver ErpApiDriver = null!;
    protected TaxApiDriver TaxApiDriver = null!;

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
        ErpApiDriver.CreateProduct(sku, "20.00");
        var placeOrderResult = ShopDriver.PlaceOrder(sku, "5", "US");
        AssertThatResult(placeOrderResult).IsSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;

        orderNumber.Should().StartWith("ORD-");

        var viewOrderResult = ShopDriver.ViewOrder(orderNumber!);
        AssertThatResult(viewOrderResult).IsSuccess();

        var viewOrderResponse = viewOrderResult.GetValue();
        viewOrderResponse.OrderNumber.Should().Be(orderNumber);
        viewOrderResponse.Sku.Should().Be(sku);
        viewOrderResponse.Quantity.Should().Be(5);
        viewOrderResponse.Country.Should().Be("US");
        viewOrderResponse.UnitPrice.Should().Be(20.00m);
        viewOrderResponse.OriginalPrice.Should().Be(100.00m);
        viewOrderResponse.Status.Should().Be(OrderStatus.PLACED);

        var discountRate = viewOrderResponse.DiscountRate;
        var discountAmount = viewOrderResponse.DiscountAmount;
        var subtotalPrice = viewOrderResponse.SubtotalPrice;

        discountRate.Should().BeGreaterThanOrEqualTo(0m);
        discountAmount.Should().BeGreaterThanOrEqualTo(0m);
        subtotalPrice.Should().BeGreaterThan(0m);

        var taxRate = viewOrderResponse.TaxRate;
        var taxAmount = viewOrderResponse.TaxAmount;
        var totalPrice = viewOrderResponse.TotalPrice;

        taxRate.Should().BeGreaterThanOrEqualTo(0m, "Tax rate should be non-negative");
        taxAmount.Should().BeGreaterThanOrEqualTo(0m, "Tax amount should be non-negative");
        totalPrice.Should().BeGreaterThan(0m, "Total price should be positive");
    }

    [Fact]
    public void ShouldCancelOrder()
    {
        var sku = "XYZ-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "50.00");
        var placeOrderResult = ShopDriver.PlaceOrder(sku, "2", "US");
        AssertThatResult(placeOrderResult).IsSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;
        var cancelOrderResult = ShopDriver.CancelOrder(orderNumber!);
        AssertThatResult(cancelOrderResult).IsSuccess();

        var viewOrderResult = ShopDriver.ViewOrder(orderNumber!);
        AssertThatResult(viewOrderResult).IsSuccess();

        var viewOrderResponse = viewOrderResult.GetValue();
        viewOrderResponse.OrderNumber.Should().Be(orderNumber);
        viewOrderResponse.Sku.Should().Be(sku);
        viewOrderResponse.Quantity.Should().Be(2);
        viewOrderResponse.Country.Should().Be("US");
        viewOrderResponse.UnitPrice.Should().Be(50.00m);
        viewOrderResponse.OriginalPrice.Should().Be(100.00m);
        viewOrderResponse.Status.Should().Be(OrderStatus.CANCELLED);
    }

    [Fact]
    public void ShouldRejectOrderWithNonExistentSku()
    {
        var result = ShopDriver.PlaceOrder("NON-EXISTENT-SKU-12345", "5", "US");
        AssertThatResult(result).IsFailure("Product does not exist for SKU: NON-EXISTENT-SKU-12345");
    }

    [Fact]
    public void ShouldNotBeAbleToViewNonExistentOrder()
    {
        var result = ShopDriver.ViewOrder("NON-EXISTENT-ORDER-12345");
        AssertThatResult(result).IsFailure("Order NON-EXISTENT-ORDER-12345 does not exist.");
    }

    [Fact]
    public void ShouldRejectOrderWithNegativeQuantity()
    {
        var sku = "DEF-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "30.00");
        var result = ShopDriver.PlaceOrder(sku, "-3", "US");
        AssertThatResult(result).IsFailure("Quantity must be positive");
    }

    [Fact]
    public void ShouldRejectOrderWithZeroQuantity()
    {
        var sku = "GHI-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "40.00");
        var result = ShopDriver.PlaceOrder(sku, "0", "US");
        AssertThatResult(result).IsFailure("Quantity must be positive");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptySku(string sku)
    {
        var result = ShopDriver.PlaceOrder(sku, "5", "US");
        AssertThatResult(result).IsFailure("SKU must not be empty");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptyQuantity(string emptyQuantity)
    {
        var result = ShopDriver.PlaceOrder("some-sku", emptyQuantity, "US");
        AssertThatResult(result).IsFailure("Quantity must not be empty");
    }

    [Theory]
    [InlineData("3.5")]
    [InlineData("lala")]
    public void ShouldRejectOrderWithNonIntegerQuantity(string nonIntegerQuantity)
    {
        var result = ShopDriver.PlaceOrder("some-sku", nonIntegerQuantity, "US");
        AssertThatResult(result).IsFailure("Quantity must be an integer");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRejectOrderWithEmptyCountry(string emptyCountry)
    {
        var result = ShopDriver.PlaceOrder("some-sku", "5", emptyCountry);
        AssertThatResult(result).IsFailure("Country must not be empty");
    }
}
