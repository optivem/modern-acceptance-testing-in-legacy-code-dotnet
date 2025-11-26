using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers;
using Optivem.AtddAccelerator.EShop.SystemTest.Core.Drivers.System;

namespace Optivem.AtddAccelerator.EShop.SystemTest.E2eTests;

public class ApiE2eTest : BaseE2eTest
{
    protected override IShopDriver CreateDriver()
    {
        return DriverFactory.CreateShopApiDriver();
    }

    // [Fact]
    // public void ShouldRejectOrderWithNullQuantity()
    // {
    //     var result = ShopDriver.PlaceOrder("some-sku", null!, "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Quantity must not be empty", result.Errors);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithNullSku()
    // {
    //     var result = ShopDriver.PlaceOrder(null!, "5", "US");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("SKU must not be empty", result.Errors);
    // }

    // [Fact]
    // public void ShouldRejectOrderWithNullCountry()
    // {
    //     var result = ShopDriver.PlaceOrder("some-sku", "5", null!);
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Country must not be empty", result.Errors);
    // }

    // [Fact]
    // public void ShouldNotCancelNonExistentOrder()
    // {
    //     var result = ShopDriver.CancelOrder("NON-EXISTENT-ORDER-99999");
    //     Assert.True(result.IsFailure);
    //     Assert.Contains("Order NON-EXISTENT-ORDER-99999 does not exist.", result.Errors);
    // }

    // [Fact]
    // public void ShouldNotCancelAlreadyCancelledOrder()
    // {
    //     var sku = $"MNO-{Guid.NewGuid()}";
    //     ErpApiDriver.CreateProduct(sku, "35.00");
    //     var placeOrderResult = ShopDriver.PlaceOrder(sku, "3", "US");
    //     Assert.True(placeOrderResult.Success);

    //     var orderNumber = placeOrderResult.Value!.OrderNumber!;

    //     // Cancel the order first time - should succeed
    //     var firstCancelResult = ShopDriver.CancelOrder(orderNumber);
    //     Assert.True(firstCancelResult.Success);

    //     // Try to cancel the same order again - should fail
    //     var secondCancelResult = ShopDriver.CancelOrder(orderNumber);
    //     Assert.True(secondCancelResult.IsFailure);
    //     Assert.Contains("Order has already been cancelled", secondCancelResult.Errors);
    // }
}
