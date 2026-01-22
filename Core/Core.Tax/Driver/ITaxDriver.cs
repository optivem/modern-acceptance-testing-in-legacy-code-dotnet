using Optivem.Commons.Util;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Tax.Driver.Dtos.Error;

namespace Optivem.EShop.SystemTest.Core.Tax.Driver;

public interface ITaxDriver : IDisposable
{
    Result<VoidValue, TaxErrorResponse> GoToTax();
    Result<GetTaxResponse, TaxErrorResponse> GetTaxRate(string? country);
    Result<VoidValue, TaxErrorResponse> ReturnsTaxRate(ReturnsTaxRateRequest request);
}