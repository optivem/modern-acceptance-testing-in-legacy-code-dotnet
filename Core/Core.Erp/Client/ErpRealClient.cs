using Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Client;

public class ErpRealClient : BaseErpClient
{
    public ErpRealClient(string baseUrl) : base(baseUrl)
    {
    }
    

    public async Task<Result<VoidValue, ExtErpErrorResponse>> CreateProduct(ExtCreateProductRequest request)
    {
        return await HttpClient.Post("/api/products", request);
    }
}