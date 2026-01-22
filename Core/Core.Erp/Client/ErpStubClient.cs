using Optivem.Commons.Util;
using Optivem.Commons.WireMock;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

/// <summary>
/// Stub ERP client for making HTTP calls to the WireMock stub.
/// </summary>
public class ErpStubClient : BaseErpClient
{
    private readonly JsonWireMockClient _wireMockClient;

    public ErpStubClient(string baseUrl) : base(baseUrl)
    {
        _wireMockClient = new JsonWireMockClient(baseUrl);
    }

    public override Result<ExtProductDetailsResponse, ExtErpErrorResponse> GetProduct(string id)
    {
        return HttpClient.Get<ExtProductDetailsResponse>($"/api/products/{id}");
    }

    public Result<VoidValue, ExtErpErrorResponse> ConfigureGetProduct(ExtProductDetailsResponse response)
    {
        var sku = response.Id;
        return _wireMockClient.StubGet($"/erp/api/products/{sku}", 200, response)
            .MapFailure(error => new ExtErpErrorResponse { Message = error });
    }
}