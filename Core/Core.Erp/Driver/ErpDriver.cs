using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Erp.Driver;

public interface IErpDriver : IDisposable
{
    Result<VoidValue, ErpErrorResponse> GoToErp();

    Result<GetProductResponse, ErpErrorResponse> GetProduct(GetProductRequest request);

    Result<VoidValue, ErpErrorResponse> ReturnsProduct(ReturnsProductRequest request);
}
