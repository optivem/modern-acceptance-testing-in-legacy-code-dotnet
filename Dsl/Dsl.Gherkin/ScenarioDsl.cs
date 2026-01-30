using Dsl.Gherkin.Given;
using Dsl.Gherkin.When;
using Optivem.EShop.SystemTest.Core;
using Optivem.Testing;
using System;

namespace Dsl.Gherkin;

public class ScenarioDsl : IAsyncDisposable
{
    private readonly Channel _channel;
    private readonly SystemDsl _app;

    private bool _executed = false;

    public ScenarioDsl(Channel channel, SystemDsl app)
    {
        _channel = channel;
        _app = app;
    }

    internal Channel Channel => _channel;

    public GivenClause Given()
    {
        EnsureNotExecuted();
        return new GivenClause(_channel, _app, this);
    }

    public WhenClause When()
    {
        EnsureNotExecuted();
        return new WhenClause(_channel, _app, this);
    }

    public void MarkAsExecuted()
    {
        _executed = true;
    }

    private void EnsureNotExecuted()
    {
        if (_executed)
        {
            throw new InvalidOperationException("Scenario has already been executed. " +
                "Each test method should contain only ONE scenario execution (Given-When-Then). " +
                "Split multiple scenarios into separate test methods.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }
}