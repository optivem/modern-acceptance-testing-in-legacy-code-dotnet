using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Entities;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_number")]
    [Required]
    public string OrderNumber { get; set; } = null!;

    [Column("order_timestamp")]
    [Required]
    public DateTime OrderTimestamp { get; set; }

    [Column("country")]
    [Required]
    public string Country { get; set; } = null!;

    [Column("sku")]
    [Required]
    public string Sku { get; set; } = null!;

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; }

    [Column("unit_price")]
    [Required]
    public decimal UnitPrice { get; set; }

    [Column("original_price")]
    [Required]
    public decimal OriginalPrice { get; set; }

    [Column("discount_rate")]
    [Required]
    public decimal DiscountRate { get; set; }

    [Column("discount_amount")]
    [Required]
    public decimal DiscountAmount { get; set; }

    [Column("subtotal_price")]
    [Required]
    public decimal SubtotalPrice { get; set; }

    [Column("tax_rate")]
    [Required]
    public decimal TaxRate { get; set; }

    [Column("tax_amount")]
    [Required]
    public decimal TaxAmount { get; set; }

    [Column("total_price")]
    [Required]
    public decimal TotalPrice { get; set; }

    [Column("status")]
    [Required]
    public OrderStatus Status { get; set; }

    public Order()
    {
    }

    public Order(string orderNumber, DateTime orderTimestamp, string country,
                 string sku, int quantity, decimal unitPrice, decimal originalPrice,
                 decimal discountRate, decimal discountAmount, decimal subtotalPrice,
                 decimal taxRate, decimal taxAmount, decimal totalPrice, OrderStatus status)
    {
        if (orderNumber == null)
            throw new ArgumentNullException(nameof(orderNumber), "orderNumber cannot be null");
        if (orderTimestamp == default)
            throw new ArgumentException("orderTimestamp cannot be default", nameof(orderTimestamp));
        if (country == null)
            throw new ArgumentNullException(nameof(country), "country cannot be null");
        if (sku == null)
            throw new ArgumentNullException(nameof(sku), "sku cannot be null");

        OrderNumber = orderNumber;
        OrderTimestamp = orderTimestamp;
        Country = country;
        Sku = sku;
        Quantity = quantity;
        UnitPrice = unitPrice;
        OriginalPrice = originalPrice;
        DiscountRate = discountRate;
        DiscountAmount = discountAmount;
        SubtotalPrice = subtotalPrice;
        TaxRate = taxRate;
        TaxAmount = taxAmount;
        TotalPrice = totalPrice;
        Status = status;
    }
}
