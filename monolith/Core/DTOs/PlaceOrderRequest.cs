using System.ComponentModel.DataAnnotations;

namespace Optivem.AtddAccelerator.EShop.Monolith.Core.DTOs;

public class PlaceOrderRequest
{
    [Required(ErrorMessage = "SKU must not be empty")]
    public string Sku { get; set; } = null!;

    [Required(ErrorMessage = "Quantity must not be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Country must not be empty")]
    public string Country { get; set; } = null!;
}
