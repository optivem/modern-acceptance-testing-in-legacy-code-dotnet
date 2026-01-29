using Commons.Util;
using Commons.WireMock;
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

    public async Task<Result<VoidValue, ExtTaxErrorResponse>> ConfigureGetCountry(ExtCountryDetailsResponse response)
        => (await _wireMockClient.StubGetAsync($"/tax/api/countries/{response.Id}", 200, response))
            .MapError(ExtTaxErrorResponse.From);
}