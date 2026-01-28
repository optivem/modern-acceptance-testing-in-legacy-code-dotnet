using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Clock.Driver.Dtos;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;

public class ReturnsTime : BaseClockCommand<VoidValue, VoidVerification>
{
    private string? _time;

    public ReturnsTime(IClockDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public ReturnsTime Time(string? time)
    {
        _time = time;
        return this;
    }

    public override ClockUseCaseResult<VoidValue, VoidVerification> Execute()
    {
        var request = new ReturnsTimeRequest
        {
            Time = _time
        };

        var result = _driver.ReturnsTime(request);
        
        return new ClockUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context,
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
