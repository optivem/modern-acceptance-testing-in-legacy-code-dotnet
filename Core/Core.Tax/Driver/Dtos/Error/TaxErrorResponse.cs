using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;

public class TaxErrorResponse
{
    public string? Message { get; set; }

    public static TaxErrorResponse From(ExtTaxErrorResponse errorResponse)
    {
        return new TaxErrorResponse
        {
            Message = errorResponse.Message
        };
    }

    public override string ToString()
    {
        return Message ?? string.Empty;
    }
}