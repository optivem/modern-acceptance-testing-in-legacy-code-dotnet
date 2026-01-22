namespace Optivem.EShop.SystemTest.Core.Tax.Client.Dtos.Error;

public class ExtTaxErrorResponse
{
    public string? Message { get; set; }
    
    public static ExtTaxErrorResponse From(string message)
    {
        return new ExtTaxErrorResponse 
        {
            Message = message
        };
    }
}