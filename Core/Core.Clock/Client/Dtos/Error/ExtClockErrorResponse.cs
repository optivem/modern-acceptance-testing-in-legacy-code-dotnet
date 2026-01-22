namespace Optivem.EShop.SystemTest.Core.Clock.Client.Dtos.Error;

public class ExtClockErrorResponse
{
    public string? Message { get; set; }

    public static ExtClockErrorResponse From(string message)
    {
        return new ExtClockErrorResponse { Message = message };
    }
}