using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class SuccessVerificationBuilder
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;

    public SuccessVerificationBuilder(SystemDsl systemDsl, Channel channel)
    {
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public void Verify()
    {
        // Success verification is implicit - if we got here without exception, it succeeded
    }
}
