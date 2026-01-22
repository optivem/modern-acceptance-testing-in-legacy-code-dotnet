using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Requests;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public class ErpDriver : IDisposable
{
    private readonly ErpRealClient _erpClient;

    public ErpDriver(string baseUrl)
    {
        _erpClient = new ErpRealClient(baseUrl);
    }

    public Result<VoidValue, ErpErrorResponse> GoToErp()
    {
        return _erpClient.CheckHealth()
            .MapFailure(ErpErrorResponse.From);
    }

    public Result<VoidValue, ErpErrorResponse> CreateProduct(ExtCreateProductRequest request)
    {
        return _erpClient.CreateProduct(request)
            .MapFailure(ErpErrorResponse.From);
    }

    public void Dispose()
    {
        _erpClient?.Dispose();
    }
}
