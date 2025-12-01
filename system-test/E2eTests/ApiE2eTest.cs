using Optivem.EShop.SystemTest.Core.Drivers;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.System;
using Xunit;

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
        ShopDriver.PlaceOrder("some-sku", null, "US")
            .ShouldBeFailure("Quantity must not be empty");
    }

    [Fact]
    public void ShouldRejectOrderWithNullSku()
    {
        ShopDriver.PlaceOrder(null, "5", "US")
            .ShouldBeFailure("SKU must not be empty");
    }

    [Fact]
    public void ShouldRejectOrderWithNullCountry()
    {
        ShopDriver.PlaceOrder("some-sku", "5", null)
            .ShouldBeFailure("Country must not be empty");
    }

    [Fact]
    public void ShouldNotCancelNonExistentOrder()
    {
        ShopDriver.CancelOrder("NON-EXISTENT-ORDER-99999")
            .ShouldBeFailure("Order NON-EXISTENT-ORDER-99999 does not exist.");
    }

    [Fact]
    public void ShouldNotCancelAlreadyCancelledOrder()
    {
        var sku = "MNO-" + Guid.NewGuid();
        ErpApiDriver.CreateProduct(sku, "35.00").ShouldBeSuccess();

        var placeOrderResult = ShopDriver.PlaceOrder(sku, "3", "US").ShouldBeSuccess();

        var orderNumber = placeOrderResult.GetValue().OrderNumber;

        // Cancel the order first time - should succeed
        ShopDriver.CancelOrder(orderNumber).ShouldBeSuccess();

        // Try to cancel the same order again - should fail
        ShopDriver.CancelOrder(orderNumber)
            .ShouldBeFailure("Order has already been cancelled");
    }
}
