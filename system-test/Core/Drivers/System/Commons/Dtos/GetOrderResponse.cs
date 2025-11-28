using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Enums;

namespace Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;

public class GetOrderResponse
{
    public string? OrderNumber { get; set; }
    public string? Sku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubtotalPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public string? Country { get; set; }
}
