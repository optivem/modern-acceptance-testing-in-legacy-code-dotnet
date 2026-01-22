using Optivem.Commons.Util;
using Optivem.Commons.WireMock;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Client;

public class TaxStubClient : BaseTaxClient
{
    private readonly JsonWireMockClient _wireMockClient;

    public TaxStubClient(string baseUrl) : base(baseUrl)
    {
        _wireMockClient = new JsonWireMockClient(baseUrl);
    }

    public Result<VoidValue, ExtTaxErrorResponse> ConfigureGetCountry(ExtCountryDetailsResponse response)
    {
        var country = response.Id;
        return _wireMockClient.StubGet($"/tax/api/countries/{country}", 200, response)
            .MapFailure(ExtTaxErrorResponse.From);
    }
}