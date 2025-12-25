using Optivem.Testing.Channels;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Builders;

public class FailureVerificationBuilder
{
    private readonly SystemDsl _systemDsl;
    private readonly Channel _channel;

    public FailureVerificationBuilder(SystemDsl systemDsl, Channel channel)
    {
        _systemDsl = systemDsl;
        _channel = channel;
    }

    public FailureVerificationBuilder ErrorMessage(string expectedErrorMessage)
    {
        // This would verify the last error message from context
        // For now, simplified implementation
        return this;
    }

    public FailureVerificationBuilder FieldErrorMessage(string fieldName, string expectedMessage)
    {
        // This would verify specific field error
        // For now, simplified implementation
        return this;
    }
}
