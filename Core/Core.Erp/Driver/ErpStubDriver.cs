using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

/// <summary>
/// ErpStubDriver uses WireMock to stub ERP API responses.
/// This allows tests to run without a real ERP system.
/// </summary>
public class ErpStubDriver : BaseErpDriver<ErpStubClient>
{
    public ErpStubDriver(string baseUrl) : base(new ErpStubClient(baseUrl))
    {
    }

    public override Result<VoidValue, ErpErrorResponse> ReturnsProduct(ReturnsProductRequest request)
    {
        var extProductDetailsResponse = new ExtProductDetailsResponse
        {
            Id = request.Sku,
            Price = decimal.Parse(request.Price)
        };

        return _client.ConfigureGetProduct(extProductDetailsResponse)
            .MapFailure(ErpErrorResponse.From);
    }
}