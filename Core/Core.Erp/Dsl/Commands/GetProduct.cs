using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Verifications;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class GetProduct : BaseErpCommand<GetProductResponse, GetProductVerification>
{
    private string? _skuParamAlias;

    public GetProduct(IErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public GetProduct Sku(string skuParamAlias)
    {
        _skuParamAlias = skuParamAlias;
        return this;
    }

    public override async Task<UseCaseResult<GetProductResponse, ErpErrorResponse, GetProductVerification, ErpErrorVerification>> Execute()
    {
        var sku = _context.GetParamValue(_skuParamAlias);

        var request = new GetProductRequest
        {
            Sku = sku
        };

        var result = await _driver.GetProduct(request);

        return new ErpUseCaseResult<GetProductResponse, GetProductVerification>(
            result, 
            _context, 
            (response, ctx) => new GetProductVerification(response, ctx));
    }
}