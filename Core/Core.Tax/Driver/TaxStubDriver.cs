using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Client;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public class TaxStubDriver : BaseTaxDriver<TaxStubClient>
{
    public TaxStubDriver(string baseUrl) : base(new TaxStubClient(baseUrl))
    {
    }

    public override Result<VoidValue, TaxErrorResponse> ReturnsTaxRate(ReturnsTaxRateRequest request)
    {
        var country = request.Country!;
        var taxRate = decimal.Parse(request.TaxRate!);

        var response = new ExtCountryDetailsResponse
        {
            Id = country,
            TaxRate = taxRate,
            CountryName = country
        };

        return _client.ConfigureGetCountry(response)
            .MapError(TaxErrorResponse.From);
    }
}