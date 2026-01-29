using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Dsl.Verifications;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class GetTime : BaseClockCommand<GetTimeResponse, GetTimeVerification>
{
    public GetTime(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override async Task<UseCaseResult<GetTimeResponse, ClockErrorResponse, GetTimeVerification, ClockErrorVerification>> Execute()
    {
        var result = await _driver.GetTime();

        return new ClockUseCaseResult<GetTimeResponse, GetTimeVerification>(
            result, 
            _context,
            (response, ctx) => new GetTimeVerification(response, ctx));
    }
}
