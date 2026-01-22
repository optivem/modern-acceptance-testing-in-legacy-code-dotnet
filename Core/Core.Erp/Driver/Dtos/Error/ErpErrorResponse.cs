using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

public class ErpErrorResponse
{
    public string? Message { get; set; }

    public static ErpErrorResponse From(ExtErpErrorResponse errorResponse)
    {
        return new ErpErrorResponse { Message = errorResponse.Message };
    }
}