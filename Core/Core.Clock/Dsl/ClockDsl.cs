using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Optivem.EShop.SystemTest.Core.Common;
using Optivem.EShop.SystemTest.Core.Common.Dsl;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl;

public class ClockDsl : IDisposable
{
    private readonly IClockDriver _driver;
    private readonly UseCaseContext _context;

    public ClockDsl(UseCaseContext context, SystemConfiguration configuration)
    {
        _driver = CreateDriver(configuration);
        _context = context;
    }

    private static IClockDriver CreateDriver(SystemConfiguration configuration)
    {
        return new ClockRealDriver();
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToClock GoToClock() => new(_driver, _context);
    
    public ReturnsTime ReturnsTime() => new(_driver, _context);
    
    public GetTime GetTime() => new(_driver, _context);
}
