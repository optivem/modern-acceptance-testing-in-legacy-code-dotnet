using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Dsl.Verifications;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class GetTime : BaseClockCommand<GetTimeResponse, GetTimeVerification>
{
    public GetTime(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ClockUseCaseResult<GetTimeResponse, GetTimeVerification> Execute()
    {
        var result = _driver.GetTime();
        return new ClockUseCaseResult<GetTimeResponse, GetTimeVerification>(
            result, 
            _context,
            (response, ctx) => new GetTimeVerification(response, ctx));
    }
}
