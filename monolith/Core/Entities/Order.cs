using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Entities;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_number")]
    [MaxLength(255)]
    public string OrderNumber { get; set; } = string.Empty;

    [Column("order_timestamp")]
    public DateTime OrderTimestamp { get; set; }

    [Column("country")]
    [MaxLength(2)]
    public string Country { get; set; } = string.Empty;

    [Column("sku")]
    [MaxLength(255)]
    public string Sku { get; set; } = string.Empty;

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("unit_price")]
    [Precision(10, 2)]
    public decimal UnitPrice { get; set; }

    [Column("original_price")]
    [Precision(10, 2)]
    public decimal OriginalPrice { get; set; }

    [Column("discount_rate")]
    [Precision(5, 4)]
    public decimal DiscountRate { get; set; }

    [Column("discount_amount")]
    [Precision(10, 2)]
    public decimal DiscountAmount { get; set; }

    [Column("subtotal_price")]
    [Precision(10, 2)]
    public decimal SubtotalPrice { get; set; }

    [Column("tax_rate")]
    [Precision(5, 4)]
    public decimal TaxRate { get; set; }

    [Column("tax_amount")]
    [Precision(10, 2)]
    public decimal TaxAmount { get; set; }

    [Column("total_price")]
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public OrderStatus Status { get; set; }
}
