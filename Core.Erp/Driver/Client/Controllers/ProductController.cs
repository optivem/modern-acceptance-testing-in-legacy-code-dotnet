using Optivem.Lang;
using Optivem.Http;
using Optivem.EShop.SystemTest.Core.Common.Error;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly JsonHttpClient<ProblemDetailResponse> _httpClient;

    public ProductController(JsonHttpClient<ProblemDetailResponse> httpClient)
    {
        _httpClient = httpClient;
    }

    public Result<VoidValue, Error> CreateProduct(CreateProductRequest request)
    {
        return _httpClient.Post(Endpoint, request)
            .MapFailure(ProblemDetailConverter.ToError);
    }
}
