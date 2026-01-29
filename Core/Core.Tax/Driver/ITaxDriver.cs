using Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public interface ITaxDriver : IDisposable
{
    Task<Result<VoidValue, TaxErrorResponse>> GoToTax();
    Task<Result<GetTaxResponse, TaxErrorResponse>> GetTaxRate(string? country);
    Task<Result<VoidValue, TaxErrorResponse>> ReturnsTaxRate(ReturnsTaxRateRequest request);
}