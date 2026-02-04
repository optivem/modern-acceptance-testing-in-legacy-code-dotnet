namespace Optivem.EShop.SystemTest.ExternalSystemContractTests.Clock;

public abstract class BaseClockContractTest : BaseExternalSystemContractTest
{
    [Fact]
    public async Task ShouldBeAbleToGetTime()
    {
        (await App.Clock().ReturnsTime()
            .Time("2024-06-15T12:00:00Z")
            .Execute())
            .ShouldSucceed();

        (await App.Clock().GetTime()
            .Execute())
            .ShouldSucceed()
            .TimeIsNotNull();
    }
}
