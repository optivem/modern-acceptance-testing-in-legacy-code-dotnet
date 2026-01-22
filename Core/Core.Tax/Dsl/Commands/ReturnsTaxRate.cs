using Optivem.EShop.SystemTest.Core.Tax.Driver;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Tax.Dsl.Commands;

public class ReturnsTaxRate : BaseTaxCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    private string? countryAlias;
    private string? taxRate;

    public ReturnsTaxRate(ITaxDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public ReturnsTaxRate Country(string countryAlias)
    {
        this.countryAlias = countryAlias;
        return this;
    }

    public ReturnsTaxRate TaxRate(string taxRate)
    {
        this.taxRate = taxRate;
        return this;
    }

    public ReturnsTaxRate TaxRate(double taxRate)
    {
        return TaxRate(taxRate.ToString());
    }

    public override TaxUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var country = _context.GetParamValueOrLiteral(countryAlias);

        var request = new ReturnsTaxRateRequest
        {
            Country = country,
            TaxRate = taxRate
        };

        var result = _driver.ReturnsTaxRate(request);
        
        return new TaxUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}