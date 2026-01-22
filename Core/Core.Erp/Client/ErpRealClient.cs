using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Error;

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

    public override Result<ExtProductDetailsResponse, ExtErpErrorResponse> GetProduct(string id)
    {
        return HttpClient.Get<ExtProductDetailsResponse>($"/api/products/{id}");
    }
}