namespace Optivem.AtddAccelerator.EShop.SystemTest.Core.Clients.External.Erp.Dtos;

public class CreateProductRequest
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public string? Brand { get; set; }
}
