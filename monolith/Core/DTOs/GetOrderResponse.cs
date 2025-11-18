using System.Text.Json.Serialization;
using Optivem.EShop.Monolith.Core.Entities;

namespace Optivem.EShop.Monolith.Core.DTOs;

public class GetOrderResponse
{
    [JsonPropertyName("orderNumber")]
    public string? OrderNumber { get; set; }

    [JsonPropertyName("sku")]
    public string? Sku { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("originalPrice")]
    public decimal OriginalPrice { get; set; }

    [JsonPropertyName("discountRate")]
    public decimal DiscountRate { get; set; }

    [JsonPropertyName("discountAmount")]
    public decimal DiscountAmount { get; set; }

    [JsonPropertyName("subtotalPrice")]
    public decimal SubtotalPrice { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal TaxRate { get; set; }

    [JsonPropertyName("taxAmount")]
    public decimal TaxAmount { get; set; }

    [JsonPropertyName("totalPrice")]
    public decimal TotalPrice { get; set; }

    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
