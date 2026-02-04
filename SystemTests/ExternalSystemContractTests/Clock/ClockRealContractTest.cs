namespace Optivem.EShop.SystemTest.ExternalSystemContractTests.Clock;

public class ClockRealContractTest : BaseClockContractTest
{
    protected override ExternalSystemMode? FixedExternalSystemMode => ExternalSystemMode.Real;
}
