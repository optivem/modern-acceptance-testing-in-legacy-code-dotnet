using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Commons.Util;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class GoToClock : BaseClockCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    public GoToClock(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ClockUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToClock();
        return new ClockUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context,
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
