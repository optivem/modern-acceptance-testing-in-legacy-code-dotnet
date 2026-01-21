using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class ReturnsTime : BaseClockCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    private static readonly DateTimeOffset DefaultTime = DateTimeOffset.Parse("2025-12-24T10:00:00Z");
    
    private DateTimeOffset _time = DefaultTime;

    public ReturnsTime(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public ReturnsTime Time(DateTimeOffset time)
    {
        _time = time;
        return this;
    }

    public ReturnsTime Time(string timeString)
    {
        _time = DateTimeOffset.Parse(timeString);
        return this;
    }

    public override ClockUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var request = new ReturnsTimeRequest
        {
            Time = _time
        };

        var result = _driver.ReturnsTime(request);
        return new ClockUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context,
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
