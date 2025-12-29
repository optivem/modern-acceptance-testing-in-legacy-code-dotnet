using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;

namespace Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;

public class GetOrderResponse
{
    public required string OrderNumber { get; set; }
    public required string Sku { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
    public required decimal SubtotalPrice { get; set; }
    public required decimal DiscountRate { get; set; }
    public required decimal DiscountAmount { get; set; }
    public required decimal PreTaxTotal { get; set; }
    public required decimal TaxRate { get; set; }
    public required decimal TaxAmount { get; set; }
    public required decimal TotalPrice { get; set; }
    public required OrderStatus Status { get; set; }
    public required string Country { get; set; }
}
