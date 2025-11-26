namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Dtos;

public class CreateProductRequest
{
    public string Sku { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}
