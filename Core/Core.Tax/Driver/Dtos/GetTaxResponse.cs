namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;

public class GetTaxResponse
{
    public required string Country { get; set; }
    public decimal TaxRate { get; set; }
}