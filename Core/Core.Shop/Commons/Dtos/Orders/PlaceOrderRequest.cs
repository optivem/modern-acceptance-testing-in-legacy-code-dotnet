namespace Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;

public class PlaceOrderRequest
{
    public string? Sku { get; set; }
    public string? Quantity { get; set; }
    public string? Country { get; set; }
    public string? CouponCode { get; set; }
}