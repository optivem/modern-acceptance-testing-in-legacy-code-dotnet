namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;

public class PlaceOrderRequest
{
    public string Sku { get; set; } = null!;
    public string Quantity { get; set; } = null!;
    public string Country { get; set; } = null!;
}
