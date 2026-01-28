using Optivem.EShop.SystemTest.Core.Clock.Dsl.Commands;
using Optivem.EShop.SystemTest.Core.Clock.Driver;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Clock.Dsl;

public class ClockDsl : IDisposable
{
    private readonly IClockDriver _driver;
    private readonly UseCaseContext _context;

    private ClockDsl(IClockDriver driver, UseCaseContext context)
    {
        _driver = driver;
        _context = context;
    }

    public ClockDsl(string baseUrl, UseCaseContext context)
        : this(CreateDriver(baseUrl, context), context)
    {
    }

    private static IClockDriver CreateDriver(string baseUrl, UseCaseContext context)
    {
        return context.ExternalSystemMode switch
        {
            ExternalSystemMode.Real => new ClockRealDriver(),
            ExternalSystemMode.Stub => new ClockStubDriver(baseUrl),
            _ => throw new NotSupportedException($"External system mode '{context.ExternalSystemMode}' is not supported for ClockDsl.")
        };
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }

    public GoToClock GoToClock() => new(_driver, _context);
    
    public ReturnsTime ReturnsTime() => new(_driver, _context);
    
    public GetTime GetTime() => new(_driver, _context);
}
