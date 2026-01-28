using Dsl.Gherkin.Then;
using Commons.Dsl;
using Optivem.EShop.SystemTest.Core;
using Optivem.Testing;

namespace Dsl.Gherkin.When;

public abstract class BaseWhenBuilder<TSuccessResponse, TSuccessVerification>
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    private readonly SystemDsl _app;
    private readonly ScenarioDsl _scenario;

    protected BaseWhenBuilder(SystemDsl app, ScenarioDsl scenario)
    {
        _app = app;
        _scenario = scenario;
    }

    public ThenClause<TSuccessResponse, TSuccessVerification> Then()
    {
        var result = Execute(_app);
        return new ThenClause<TSuccessResponse, TSuccessVerification>(Channel, _app, _scenario, result);
    }

    protected abstract ExecutionResult<TSuccessResponse, TSuccessVerification> Execute(SystemDsl app);

    protected Channel Channel => _scenario.Channel;
}