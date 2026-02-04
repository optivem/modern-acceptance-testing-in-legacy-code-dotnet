namespace Optivem.EShop.SystemTest.ContractTests.Clock;

public class ClockRealContractTest : BaseClockContractTest
{
    protected override ExternalSystemMode? FixedExternalSystemMode => ExternalSystemMode.Real;
}
