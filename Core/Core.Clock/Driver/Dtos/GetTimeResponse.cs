using Optivem.EShop.SystemTest.Core.Clock.Client.Dtos;

namespace Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;

public class GetTimeResponse
{
    public DateTimeOffset Time { get; set; }

    public static GetTimeResponse From(ExtGetTimeResponse response)
    {
        return new GetTimeResponse { Time = response.Time };
    }
}
