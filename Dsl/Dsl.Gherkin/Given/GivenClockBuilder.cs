using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Gherkin;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.Given;

public class GivenClockBuilder : BaseGivenBuilder
{
    private string? _time;

    public GivenClockBuilder(GivenClause givenClause) : base(givenClause)
    {
        WithTime(DefaultTime);
    }

    public GivenClockBuilder WithTime(string time)
    {
        _time = time;
        return this;
    }

    internal override void Execute(SystemDsl app)
    {
        app.Clock().ReturnsTime()
            .Time(_time)
            .Execute()
            .ShouldSucceed();
    }
}