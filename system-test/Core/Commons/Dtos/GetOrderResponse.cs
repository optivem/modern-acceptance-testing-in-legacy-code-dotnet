using Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos.Enums;

namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Commons.Dtos;

public class GetOrderResponse
{
    public string OrderNumber { get; set; } = null!;
    public string Sku { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubtotalPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Country { get; set; } = null!;
    public OrderStatus Status { get; set; }
}
