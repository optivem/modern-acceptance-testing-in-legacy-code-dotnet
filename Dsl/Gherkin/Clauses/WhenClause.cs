using Optivem.EShop.SystemTest.Core.Gherkin.Builders;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Clauses;

public class WhenClause
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;
    private bool _whenExecuted = false;

    public WhenClause(SystemDsl systemDsl, Channel channel)
    {
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public PlaceOrderBuilder PlaceOrder()
    {
        return new PlaceOrderBuilder(this, _systemDsl, _channel);
    }

    public CancelOrderBuilder CancelOrder()
    {
        return new CancelOrderBuilder(this, _systemDsl, _channel);
    }

    public ViewOrderBuilder ViewOrder()
    {
        return new ViewOrderBuilder(this, _systemDsl, _channel);
    }

    internal void MarkWhenExecuted()
    {
        _whenExecuted = true;
    }

    public ThenClause Then()
    {
        if (!_whenExecuted)
        {
            throw new InvalidOperationException("When clause must be executed before calling Then");
        }

        return new ThenClause(_systemDsl, _channel);
    }
}
