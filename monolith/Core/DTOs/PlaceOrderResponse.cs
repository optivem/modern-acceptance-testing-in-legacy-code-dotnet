using System.Text.Json.Serialization;

namespace Optivem.EShop.Monolith.Core.DTOs;

public class PlaceOrderResponse
{
    [JsonPropertyName("orderNumber")]
    public string? OrderNumber { get; set; }
}
