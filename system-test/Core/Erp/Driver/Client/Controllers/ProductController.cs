using Optivem.Lang;
using Optivem.Http;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly JsonHttpClient<ProblemDetailResponse> _testHttpClient;

    public ProductController(JsonHttpClient<ProblemDetailResponse> testHttpClient)
    {
        _testHttpClient = testHttpClient;
    }

    public Result<VoidValue, Error> CreateProduct(CreateProductRequest request)
    {
        return _testHttpClient.Post(Endpoint, request)
            .MapFailure(ProblemDetailConverter.ToError);
    }
}
