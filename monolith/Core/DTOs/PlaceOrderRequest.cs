using Optivem.EShop.Monolith.Core.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Optivem.EShop.Monolith.Core.DTOs;

public class PlaceOrderRequest
{
    [Required(ErrorMessage = "SKU must not be empty")]
    [JsonPropertyName("sku")]
    public string? Sku { get; set; }

    [Required(ErrorMessage = "Quantity must not be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
    [JsonPropertyName("quantity")]
    [JsonConverter(typeof(EmptyStringToNullIntConverter))]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Country must not be empty")]
    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
