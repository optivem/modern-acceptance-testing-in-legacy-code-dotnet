using Optivem.Commons.Util;
using Optivem.Commons.Http;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly JsonHttpClient<ExtErpErrorResponse> _httpClient;

    public ProductController(JsonHttpClient<ExtErpErrorResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidValue, ExtErpErrorResponse> CreateProduct(CreateProductRequest request)
    {
        return _httpClient.Post(Endpoint, request);
    }
}
