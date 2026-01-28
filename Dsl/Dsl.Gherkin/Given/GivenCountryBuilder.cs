using Dsl.Gherkin.Given;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Given;

public class GivenCountryBuilder : BaseGivenBuilder
{
    private string? _country;
    private string? _taxRate;

    public GivenCountryBuilder(GivenClause givenClause) 
        : base(givenClause)
    {
        WithCode(DefaultCountry);
        WithTaxRate(DefaultTaxRate);
    }

    public GivenCountryBuilder WithCode(string country)
    {
        _country = country;
        return this;
    }

    public GivenCountryBuilder WithTaxRate(string taxRate)
    {
        _taxRate = taxRate;
        return this;
    }

    public GivenCountryBuilder WithTaxRate(decimal taxRate)
    {
        return WithTaxRate(taxRate.ToString());
    }

    internal override void Execute(SystemDsl app)
    {
        app.Tax().ReturnsTaxRate()
            .Country(_country)
            .TaxRate(_taxRate)
            .Execute()
            .ShouldSucceed();
    }
}