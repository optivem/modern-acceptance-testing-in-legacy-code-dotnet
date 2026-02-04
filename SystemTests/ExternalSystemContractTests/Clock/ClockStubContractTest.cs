namespace Optivem.EShop.SystemTest.ExternalSystemContractTests.Clock;

public class ClockStubContractTest : BaseClockContractTest
{
    protected override ExternalSystemMode? FixedExternalSystemMode => ExternalSystemMode.Stub;

    [Fact]
    public async Task ShouldBeAbleToGetConfiguredTime()
    {
        (await App.Clock().ReturnsTime()
            .Time("2024-01-02T09:00:00Z")
            .Execute())
            .ShouldSucceed();

        (await App.Clock().GetTime()
            .Execute())
            .ShouldSucceed()
            .Time("2024-01-02T09:00:00Z");
    }
}
