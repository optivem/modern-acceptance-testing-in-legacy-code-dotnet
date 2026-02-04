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
    private readonly Func<Task> _ensureDefaults;

    protected BaseWhenBuilder(SystemDsl app, ScenarioDsl scenario, Func<Task> ensureDefaults)
    {
        _app = app;
        _scenario = scenario;
        _ensureDefaults = ensureDefaults;
    }

    public ThenClause<TSuccessResponse, TSuccessVerification> Then()
    {
        return new ThenClause<TSuccessResponse, TSuccessVerification>(Channel, _app, async () =>
        {
            await _ensureDefaults();
            return await Execute(_app);
        });
    }

    protected abstract Task<ExecutionResult<TSuccessResponse, TSuccessVerification>> Execute(SystemDsl app);

    protected Channel Channel => _scenario.Channel;
}