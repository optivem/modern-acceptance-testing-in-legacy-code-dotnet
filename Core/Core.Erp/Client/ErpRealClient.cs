using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpRealClient : BaseErpClient
{
    public ErpRealClient(string baseUrl) : base(baseUrl)
    {
    }

    public Result<VoidValue, ExtErpErrorResponse> CreateProduct(ExtCreateProductRequest request)
    {
        return HttpClient.Post("/api/products", request);
    }
}