using Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpRealClient : BaseErpClient
{
    public ErpRealClient(string baseUrl) : base(baseUrl)
    {
    }
    

    public Task<Result<VoidValue, ExtErpErrorResponse>> CreateProduct(ExtCreateProductRequest request)
        => HttpClient.Post("/api/products", request);
}