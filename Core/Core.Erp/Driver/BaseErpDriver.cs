using Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Client;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public abstract class BaseErpDriver<TClient> : IErpDriver
    where TClient : BaseErpClient
{
    protected readonly TClient _client;

    protected BaseErpDriver(TClient client)
    {
        _client = client;
    }

    public virtual void Dispose()
    {
        _client?.Dispose();
    }

    public virtual async Task<Result<VoidValue, ErpErrorResponse>> GoToErp()
    {
        var result = await _client.CheckHealth();
        return result.MapError(ErpErrorResponse.From);
    }

    public virtual async Task<Result<GetProductResponse, ErpErrorResponse>> GetProduct(GetProductRequest request)
    {
        var result = await _client.GetProduct(request.Sku);
        return result
            .Map(productDetails => new GetProductResponse
            {
                Sku = productDetails.Id!,
                Price = productDetails.Price
            })
            .MapError(ErpErrorResponse.From);
    }
    
    public abstract Task<Result<VoidValue, ErpErrorResponse>> ReturnsProduct(ReturnsProductRequest request);
}