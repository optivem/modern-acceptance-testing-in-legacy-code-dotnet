using Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Client;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public abstract class BaseTaxDriver<TClient> : ITaxDriver where TClient : BaseTaxClient
{
    protected readonly TClient _client;

    protected BaseTaxDriver(TClient client)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    public virtual Result<VoidValue, TaxErrorResponse> GoToTax()
    {
        var result = _client.CheckHealth().GetAwaiter().GetResult();
        return result.MapError(TaxErrorResponse.From);
    }

    public virtual Result<GetTaxResponse, TaxErrorResponse> GetTaxRate(string? country)
    {
        var result = _client.GetCountry(country).GetAwaiter().GetResult();
        return result
            .Map(taxRateResponse => new GetTaxResponse
            {
                Country = taxRateResponse.Id,
                TaxRate = taxRateResponse.TaxRate
            })
            .MapError(TaxErrorResponse.From);
    }

    public abstract Result<VoidValue, TaxErrorResponse> ReturnsTaxRate(ReturnsTaxRateRequest request);
}