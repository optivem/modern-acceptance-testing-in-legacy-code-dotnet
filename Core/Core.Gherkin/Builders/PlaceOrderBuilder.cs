using Optivem.EShop.SystemTest.Core.Gherkin.Clauses;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class PlaceOrderBuilder
{
    private readonly WhenClause _whenClause;
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;
    private string? _orderNumber;
    private string? _sku;
    private int? _quantity;
    private string? _quantityString;

    public PlaceOrderBuilder(WhenClause whenClause, SystemDsl systemDsl, Channel channel)
    {
        _whenClause = whenClause;
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public PlaceOrderBuilder OrderNumber(string orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    public PlaceOrderBuilder Sku(string sku)
    {
        _sku = sku;
        return this;
    }

    public PlaceOrderBuilder Quantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public PlaceOrderBuilder Quantity(string quantity)
    {
        _quantityString = quantity;
        return this;
    }

    public WhenClause Execute()
    {
        var command = _systemDsl.Shop(_channel).PlaceOrder();

        if (_orderNumber != null)
        {
            command.OrderNumber(_orderNumber);
        }

        if (_sku != null)
        {
            command.Sku(_sku);
        }

        if (_quantity.HasValue)
        {
            command.Quantity(_quantity.Value);
        }
        else if (_quantityString != null)
        {
            command.Quantity(_quantityString);
        }

        command.Execute();
        _whenClause.MarkWhenExecuted();
        return _whenClause;
    }
}
