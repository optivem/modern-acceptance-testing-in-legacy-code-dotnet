using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class GoToClock : BaseClockCommand<VoidValue, VoidVerification>
{
    public GoToClock(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ClockUseCaseResult<VoidValue, VoidVerification> Execute()
    {
        var result = _driver.GoToClock();

        return new ClockUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context,
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
