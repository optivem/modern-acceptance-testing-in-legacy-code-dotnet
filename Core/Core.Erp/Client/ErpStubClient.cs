using Commons.Util;
using Commons.WireMock;
using Commons.Http;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpStubClient : BaseErpClient
{
    private const string ErpProductsEndpoint = "/erp/api/products";

    private readonly JsonWireMockClient _wireMockClient;

    public ErpStubClient(string baseUrl) : base(baseUrl)
    {
        _wireMockClient = new JsonWireMockClient(baseUrl);
    }

    public new void Dispose()
    {
        base.Dispose();
        _wireMockClient?.Dispose();
    }

    public Task<Result<VoidValue, ExtErpErrorResponse>> ConfigureGetProduct(ExtProductDetailsResponse response)
        => _wireMockClient.StubGetAsync($"{ErpProductsEndpoint}/{response.Id}", HttpStatus.Ok, response)
            .MapErrorAsync(ExtErpErrorResponse.From);

}