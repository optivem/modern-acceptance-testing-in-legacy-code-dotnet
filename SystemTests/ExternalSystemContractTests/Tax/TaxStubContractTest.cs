namespace Optivem.EShop.SystemTest.ExternalSystemContractTests.Tax;

public class TaxStubContractTest : BaseTaxContractTest
{
    protected override Configuration.Environment? GetFixedEnvironment() => Configuration.Environment.Local;

    protected override ExternalSystemMode? FixedExternalSystemMode => ExternalSystemMode.Stub;
    
    [Fact]
    public async Task ShouldBeAbleToGetConfiguredTaxRate()
    {
        (await App.Tax().ReturnsTaxRate()
            .Country("LALA")
            .TaxRate("0.23")
            .Execute())
            .ShouldSucceed();

        (await App.Tax().GetTaxRate()
            .Country("LALA")
            .Execute())
            .ShouldSucceed()
            .Country("LALA")
            .TaxRate("0.23");
    }
}
