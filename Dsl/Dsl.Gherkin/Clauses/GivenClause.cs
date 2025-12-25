using Optivem.EShop.SystemTest.Core.Gherkin.Builders;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Clauses;

public class GivenClause
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;
    private readonly List<Action> _givenActions = new();

    public GivenClause(SystemDsl systemDsl, Channel channel)
    {
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public ProductBuilder Product()
    {
        return new ProductBuilder(this, _systemDsl);
    }

    internal void AddGivenAction(Action action)
    {
        _givenActions.Add(action);
    }

    public WhenClause When()
    {
        // Execute all given actions
        foreach (var action in _givenActions)
        {
            action();
        }

        return new WhenClause(_systemDsl, _channel);
    }
}
