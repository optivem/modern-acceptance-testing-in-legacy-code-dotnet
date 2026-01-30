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

    public virtual Task<Result<VoidValue, ErpErrorResponse>> GoToErp()
        => _client.CheckHealth()
            .MapErrorAsync(ErpErrorResponse.From);

    public virtual Task<Result<GetProductResponse, ErpErrorResponse>> GetProduct(GetProductRequest request)
        => _client.GetProduct(request.Sku)
            .MapAsync(productDetails => new GetProductResponse
            {
                Sku = productDetails.Id!,
                Price = productDetails.Price
            })
            .MapErrorAsync(ErpErrorResponse.From);
    
    public abstract Task<Result<VoidValue, ErpErrorResponse>> ReturnsProduct(ReturnsProductRequest request);
}