using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Xunit;
using static Optivem.EShop.SystemTest.Core.Drivers.Commons.ResultAssert;

namespace Optivem.EShop.SystemTest.E2eTests;

public class ApiE2eTest : BaseE2eTest
{
    protected override IShopDriver CreateDriver()
    {
        return DriverFactory.CreateShopApiDriver();
    }

    [Fact]
    public void ShouldRejectOrderWithNullQuantity()
    {
        var result = ShopDriver.PlaceOrder("some-sku", null!, "US");
        AssertThatResult(result).IsFailure("Quantity must not be empty");
    }

    [Fact]
    public void ShouldRejectOrderWithNullSku()
    {
        var result = ShopDriver.PlaceOrder(null!, "5", "US");
        AssertThatResult(result).IsFailure("SKU must not be empty");
    }

    [Fact]
    public void ShouldRejectOrderWithNullCountry()
    {
        var result = ShopDriver.PlaceOrder("some-sku", "5", null!);
        AssertThatResult(result).IsFailure("Country must not be empty");
    }

    [Fact]
    public void ShouldNotCancelNonExistentOrder()
    {
        var result = ShopDriver.CancelOrder("NON-EXISTENT-ORDER-99999");
        AssertThatResult(result).IsFailure("Order NON-EXISTENT-ORDER-99999 does not exist.");
    }

    [Fact]
    public void ShouldNotCancelAlreadyCancelledOrder()
    {
        var sku = "MNO-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "35.00");
        var placeOrderResult = ShopDriver.PlaceOrder(sku, "3", "US");
        AssertThatResult(placeOrderResult).IsSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;

        // Cancel the order first time - should succeed
        var firstCancelResult = ShopDriver.CancelOrder(orderNumber!);
        AssertThatResult(firstCancelResult).IsSuccess();

        // Try to cancel the same order again - should fail
        var secondCancelResult = ShopDriver.CancelOrder(orderNumber!);
        AssertThatResult(secondCancelResult).IsFailure("Order has already been cancelled");
    }
}
