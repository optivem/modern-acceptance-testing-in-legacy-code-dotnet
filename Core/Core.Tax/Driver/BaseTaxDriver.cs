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

    public virtual Task<Result<VoidValue, TaxErrorResponse>> GoToTax()
        => _client.CheckHealth()
            .MapErrorAsync(TaxErrorResponse.From);

    public virtual Task<Result<GetTaxResponse, TaxErrorResponse>> GetTaxRate(string? country)
        => _client.GetCountry(country)
            .MapAsync(taxRateResponse => new GetTaxResponse
            {
                Country = taxRateResponse.Id,
                TaxRate = taxRateResponse.TaxRate
            })
            .MapErrorAsync(TaxErrorResponse.From);

    public abstract Task<Result<VoidValue, TaxErrorResponse>> ReturnsTaxRate(ReturnsTaxRateRequest request);
}