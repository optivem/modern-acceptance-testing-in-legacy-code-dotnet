using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos.Enums;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.External;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public abstract class BaseE2eTest : IAsyncLifetime
{
    protected IShopDriver ShopDriver = default!;
    protected ErpApiDriver ErpApiDriver = default!;
    protected TaxApiDriver TaxApiDriver = default!;

    public Task InitializeAsync()
    {
        ShopDriver = CreateDriver();
        ErpApiDriver = DriverFactory.CreateErpApiDriver();
        TaxApiDriver = DriverFactory.CreateTaxApiDriver();
        return Task.CompletedTask;
    }

    protected abstract IShopDriver CreateDriver();

    public Task DisposeAsync()
    {
        DriverCloser.Close(ShopDriver);
        DriverCloser.Close(ErpApiDriver);
        DriverCloser.Close(TaxApiDriver);
        return Task.CompletedTask;
    }

    // [Fact]
    // public void ShouldPlaceOrderAndCalculateOriginalPrice()
    // {
    //     var sku = $"ABC-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "20.00");
    //     var placeOrderResult = ShopDriver.PlaceOrder(sku, "5", "US");
    //     Assert.True(placeOrderResult.Success);

    //     var orderNumber = placeOrderResult.Value!.OrderNumber;

    //     Assert.StartsWith("ORD-", orderNumber);

    //     var viewOrderResult = ShopDriver.ViewOrder(orderNumber!);
    //     Assert.True(viewOrderResult.Success);

    //     var viewOrderResponse = viewOrderResult.Value!;
    //     Assert.Equal(orderNumber, viewOrderResponse.OrderNumber);
    //     Assert.Equal(sku, viewOrderResponse.Sku);
    //     Assert.Equal(5, viewOrderResponse.Quantity);
    //     Assert.Equal("US", viewOrderResponse.Country);

    //     Assert.Equal(20.00m, viewOrderResponse.UnitPrice);
    //     Assert.Equal(100.00m, viewOrderResponse.OriginalPrice);
    //     Assert.Equal(OrderStatus.PLACED, viewOrderResponse.Status);

    //     var discountRate = viewOrderResponse.DiscountRate;
    //     var discountAmount = viewOrderResponse.DiscountAmount;
    //     var subtotalPrice = viewOrderResponse.SubtotalPrice;

    //     Assert.True(discountRate >= 0m, "Discount rate should be non-negative");
    //     Assert.True(discountAmount >= 0m, "Discount amount should be non-negative");
    //     Assert.True(subtotalPrice > 0m, "Subtotal price should be positive");

    //     var taxRate = viewOrderResponse.TaxRate;
    //     var taxAmount = viewOrderResponse.TaxAmount;
    //     var totalPrice = viewOrderResponse.TotalPrice;

    //     Assert.True(taxRate >= 0m, "Tax rate should be non-negative");
    //     Assert.True(taxAmount >= 0m, "Tax amount should be non-negative");
    //     Assert.True(totalPrice > 0m, "Total price should be positive");
    // }

    // [Fact]
    // public void ShouldCancelOrder()
    // {
    //     var sku = $"XYZ-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "50.00");
    //     var placeOrderResult = ShopDriver.PlaceOrder(sku, "2", "US");
    //     Assert.True(placeOrderResult.Success);

    //     var orderNumber = placeOrderResult.Value!.OrderNumber!;
    //     var cancelOrderResult = ShopDriver.CancelOrder(orderNumber);
    //     Assert.True(cancelOrderResult.Success);

    //     var viewOrderResult = ShopDriver.ViewOrder(orderNumber);
    //     Assert.True(viewOrderResult.Success);

    //     var viewOrderResponse = viewOrderResult.Value!;
    //     Assert.Equal(orderNumber, viewOrderResponse.OrderNumber);
    //     Assert.Equal(sku, viewOrderResponse.Sku);
    //     Assert.Equal(2, viewOrderResponse.Quantity);
    //     Assert.Equal("US", viewOrderResponse.Country);

    //     Assert.Equal(50.00m, viewOrderResponse.UnitPrice);
    //     Assert.Equal(100.00m, viewOrderResponse.OriginalPrice);
    //     Assert.Equal(OrderStatus.CANCELLED, viewOrderResponse.Status);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithNonExistentSku()
    // {
    //     var result = ShopDriver.PlaceOrder("NON-EXISTENT-SKU-12345", "5", "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Product does not exist for SKU: NON-EXISTENT-SKU-12345", result.Errors);
    // }

    // [Fact]
    // public void ShouldNotBeAbleToViewNonExistentOrder()
    // {
    //     var result = ShopDriver.ViewOrder("NON-EXISTENT-ORDER-12345");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Order NON-EXISTENT-ORDER-12345 does not exist.", result.Errors);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithNegativeQuantity()
    // {
    //     var sku = $"DEF-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "30.00");
    //     var result = ShopDriver.PlaceOrder(sku, "-3", "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Quantity must be positive", result.Errors);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithZeroQuantity()
    // {
    //     var sku = $"GHI-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "40.00");
    //     var result = ShopDriver.PlaceOrder(sku, "0", "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Quantity must be positive", result.Errors);
    // }

    // [Theory]
    // [InlineData("")]
    // [InlineData("   ")]
    // public void ShouldRejectOrderWithEmptySku(string sku)
    // {
    //     var result = ShopDriver.PlaceOrder(sku, "5", "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("SKU must not be empty", result.Errors);
    // }

    // [Theory]
    // [InlineData("")]
    // [InlineData("   ")]
    // public void ShouldRejectOrderWithEmptyQuantity(string emptyQuantity)
    // {
    //     var result = ShopDriver.PlaceOrder("some-sku", emptyQuantity, "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Quantity must not be empty", result.Errors);
    // }

    // [Theory]
    // [InlineData("3.5")]
    // [InlineData("lala")]
    // public void ShouldRejectOrderWithNonIntegerQuantity(string nonIntegerQuantity)
    // {
    //     var result = ShopDriver.PlaceOrder("some-sku", nonIntegerQuantity, "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Quantity must be an integer", result.Errors);
    // }

    // [Theory]
    // [InlineData("")]
    // [InlineData("   ")]
    // public void ShouldRejectOrderWithEmptyCountry(string emptyCountry)
    // {
    //     var result = ShopDriver.PlaceOrder("some-sku", "5", emptyCountry);
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Country must not be empty", result.Errors);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithUnsupportedCountry()
    // {
    //     var sku = $"JKL-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "25.00");
    //     var result = ShopDriver.PlaceOrder(sku, "3", "XX");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Country does not exist: XX", result.Errors);
    // }
}
