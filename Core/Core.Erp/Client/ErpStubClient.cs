using Optivem.Commons.Util;
using Optivem.Commons.WireMock;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpStubClient : BaseErpClient
{
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

    public Result<VoidValue, ExtErpErrorResponse> ConfigureGetProduct(ExtProductDetailsResponse response)
    {
        var sku = response.Id;
        return _wireMockClient.StubGet($"/erp/api/products/{sku}", 200, response)
            .MapFailure(ExtErpErrorResponse.From);
    }

}