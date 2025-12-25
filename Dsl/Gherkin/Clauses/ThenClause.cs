using Optivem.EShop.SystemTest.Core.Gherkin.Builders;
using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Clauses;

public class ThenClause
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;

    public ThenClause(SystemDsl systemDsl, Channel channel)
    {
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public SuccessVerificationBuilder Success()
    {
        return new SuccessVerificationBuilder(_systemDsl, _channel);
    }

    public FailureVerificationBuilder Failure()
    {
        return new FailureVerificationBuilder(_systemDsl, _channel);
    }

    public OrderVerificationBuilder Order(string orderNumber)
    {
        return new OrderVerificationBuilder(_systemDsl, _channel, orderNumber);
    }
}
