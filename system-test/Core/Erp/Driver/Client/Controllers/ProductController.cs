using System.Net;
using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Assertions;
using Optivem.Http;
using Optivem.Playwright;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos.Requests;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly HttpGateway _testHttpClient;

    public ProductController(HttpGateway testHttpClient)
    {
        _testHttpClient = testHttpClient;
    }

    public Result<VoidValue> CreateProduct(CreateProductRequest request)
    {
        var response = _testHttpClient.Post(Endpoint, request);

        return HttpUtils.GetCreatedResultOrFailure(response);
    }
}
