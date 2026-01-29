using Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public interface IErpDriver : IDisposable
{
    Task<Result<VoidValue, ErpErrorResponse>> GoToErp();

    Task<Result<GetProductResponse, ErpErrorResponse>> GetProduct(GetProductRequest request);

    Task<Result<VoidValue, ErpErrorResponse>> ReturnsProduct(ReturnsProductRequest request);
}
