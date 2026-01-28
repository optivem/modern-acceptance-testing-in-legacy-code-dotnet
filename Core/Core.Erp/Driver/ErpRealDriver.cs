using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Erp.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public class ErpRealDriver : BaseErpDriver<ErpRealClient>
{
    private const string DefaultTitle = "Test Product Title";
    private const string DefaultDescription = "Test Product Description";
    private const string DefaultCategory = "Test Category";
    private const string DefaultBrand = "Test Brand";

    public ErpRealDriver(string baseUrl) : base(new ErpRealClient(baseUrl))
    {
    }

    public override Result<VoidValue, ErpErrorResponse> ReturnsProduct(ReturnsProductRequest request)
    {
        var createProductRequest = new ExtCreateProductRequest
        {
            Id = request.Sku,
            Title = DefaultTitle,
            Description = DefaultDescription,
            Category = DefaultCategory,
            Brand = DefaultBrand,
            Price = request.Price
        };

        return _client.CreateProduct(createProductRequest).MapError(ErpErrorResponse.From);
    }
}