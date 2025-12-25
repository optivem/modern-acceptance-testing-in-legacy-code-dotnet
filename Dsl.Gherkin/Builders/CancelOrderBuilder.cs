using Optivem.EShop.SystemTest.Core.Gherkin.Clauses;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class CancelOrderBuilder
{
    private readonly WhenClause _whenClause;
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;
    private string? _orderNumber;

    public CancelOrderBuilder(WhenClause whenClause, SystemDsl systemDsl, Channel channel)
    {
        _whenClause = whenClause;
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public CancelOrderBuilder OrderNumber(string orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    public WhenClause Execute()
    {
        var command = _systemDsl.Shop(_channel).CancelOrder();

        if (_orderNumber != null)
        {
            command.OrderNumber(_orderNumber);
        }

        command.Execute();
        _whenClause.MarkWhenExecuted();
        return _whenClause;
    }
}
