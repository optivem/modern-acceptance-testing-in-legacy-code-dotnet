using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Enums;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class OrderVerificationBuilder
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;
    private readonly string _orderNumber;

    public OrderVerificationBuilder(SystemDsl systemDsl, Channel channel, string orderNumber)
    {
        _systemDsl = systemDsl;
        _channel = channel;
        _orderNumber = orderNumber;
    }

    public OrderVerificationBuilder Has()
    {
        return this;
    }

    public OrderVerificationBuilder Sku(string expectedSku)
    {
        _systemDsl.Shop(_channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute()
            .ShouldSucceed()
            .Sku(expectedSku);
        return this;
    }

    public OrderVerificationBuilder Quantity(int expectedQuantity)
    {
        _systemDsl.Shop(_channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute()
            .ShouldSucceed()
            .Quantity(expectedQuantity);
        return this;
    }

    public OrderVerificationBuilder Status(OrderStatus expectedStatus)
    {
        _systemDsl.Shop(_channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute()
            .ShouldSucceed()
            .Status(expectedStatus);
        return this;
    }

    public OrderVerificationBuilder SubtotalPrice(decimal expectedSubtotalPrice)
    {
        _systemDsl.Shop(_channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute()
            .ShouldSucceed()
            .SubtotalPrice(expectedSubtotalPrice);
        return this;
    }

    public OrderVerificationBuilder SubtotalPrice(string expectedSubtotalPrice)
    {
        _systemDsl.Shop(_channel).ViewOrder()
            .OrderNumber(_orderNumber)
            .Execute()
            .ShouldSucceed()
            .SubtotalPrice(expectedSubtotalPrice);
        return this;
    }
}
