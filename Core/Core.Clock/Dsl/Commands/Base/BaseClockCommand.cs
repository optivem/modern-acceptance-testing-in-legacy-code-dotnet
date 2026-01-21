using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;

public abstract class BaseClockCommand<TResponse, TVerification> 
    : BaseUseCase<IClockDriver, UseCaseContext, TResponse, ClockErrorResponse, TVerification, ClockErrorVerification>
{
    protected BaseClockCommand(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }
}
