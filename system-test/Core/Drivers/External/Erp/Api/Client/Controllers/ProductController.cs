using System.Net;
using Optivem.EShop.SystemTest.Core.Drivers.Commons;
using Optivem.EShop.SystemTest.Core.Drivers.Commons.Clients;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Dtos;

namespace Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api.Client.Controllers;

public class ProductController
{
    private const string Endpoint = "/api/products";
    private readonly TestHttpClient _testHttpClient;

    public ProductController(TestHttpClient testHttpClient)
    {
        _testHttpClient = testHttpClient;
    }

    public Result<VoidResult> CreateProduct(CreateProductRequest request)
    {
        var response = _testHttpClient.Post(Endpoint, request);

        if (response.StatusCode != HttpStatusCode.Created)
        {
            return Result<VoidResult>.FailureResult(new List<string>
            {
                $"Failed to create product. Status code: {response.StatusCode}, Body: {response.Content.ReadAsStringAsync().Result}"
            });
        }

        return Result<VoidResult>.SuccessResult(new VoidResult());
    }
}
