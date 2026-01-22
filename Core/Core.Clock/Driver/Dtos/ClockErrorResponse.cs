using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;

public class ClockErrorResponse
{
    public string? Message { get; set; }

    public static ClockErrorResponse From(ExtClockErrorResponse errorResponse)
    {
        return new ClockErrorResponse { Message = errorResponse.Message };
    }
}
