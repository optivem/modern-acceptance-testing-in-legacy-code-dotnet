using Optivem.Commons.Util;
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

    public virtual Result<VoidValue, ErpErrorResponse> GoToErp()
    {
        return _client.CheckHealth().MapFailure(ErpErrorResponse.From);
    }

    public virtual Result<GetProductResponse, ErpErrorResponse> GetProduct(GetProductRequest request)
    {
        return _client.GetProduct(request.Sku)
            .Map(productDetails => new GetProductResponse
            {
                Sku = productDetails.Id!,
                Price = productDetails.Price
            })
            .MapFailure(ErpErrorResponse.From);
    }
    
    public abstract Result<VoidValue, ErpErrorResponse> ReturnsProduct(ReturnsProductRequest request);
}