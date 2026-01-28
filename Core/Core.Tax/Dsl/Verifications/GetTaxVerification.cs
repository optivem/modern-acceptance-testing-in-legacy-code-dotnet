using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Commons.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Verifications;

public class GetTaxVerification : ResponseVerification<GetTaxResponse>
{
    public GetTaxVerification(GetTaxResponse response, UseCaseContext context) 
        : base(response, context)
    {
    }

    public GetTaxVerification ShouldHaveTaxRate(string taxRate)
    {
        Response.TaxRate.ToString().ShouldBe(taxRate);
        return this;
    }

    public GetTaxVerification ShouldHaveTaxRate(decimal taxRate)
    {
        Response.TaxRate.ShouldBe(taxRate);
        return this;
    }

    public GetTaxVerification ShouldHaveCountry(string country)
    {
        var resolvedCountry = Context.GetParamValue(country) ?? country;
        Response.Country.ShouldBe(resolvedCountry);
        return this;
    }
}