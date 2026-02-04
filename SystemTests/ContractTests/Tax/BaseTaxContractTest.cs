namespace Optivem.EShop.SystemTest.ContractTests.Tax;

public abstract class BaseTaxContractTest : BaseExternalSystemContractTest
{
    [Fact]
    public async Task ShouldBeAbleToGetTaxRate()
    {
        (await App.Tax().ReturnsTaxRate()
            .Country("US")
            .TaxRate("0.09")
            .Execute())
            .ShouldSucceed();

        (await App.Tax().GetTaxRate()
            .Country("US")
            .Execute())
            .ShouldSucceed()
            .Country("US")
            .TaxRateIsPositive();
    }
}
