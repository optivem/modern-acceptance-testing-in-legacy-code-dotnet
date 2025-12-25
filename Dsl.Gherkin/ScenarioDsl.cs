using Optivem.EShop.SystemTest.Core.Gherkin.Clauses;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin;

public class ScenarioDsl
{
    private readonly SystemDsl _systemDsl;

    public ScenarioDsl(SystemDsl systemDsl)
    {
        _systemDsl = systemDsl;
    }

    public GivenClause Given(Channel channel)
    {
        return new GivenClause(_systemDsl, channel);
    }
}
