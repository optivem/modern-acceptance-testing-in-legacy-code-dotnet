using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands.Base;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

public class CreateProduct : BaseErpCommand<object, VoidVerification>
{
    private string? _skuParamAlias;
    private string _unitPrice = "20.00";

    public CreateProduct(ErpApiDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public CreateProduct Sku(string skuParamAlias)
    {
        _skuParamAlias = skuParamAlias;
        return this;
    }

    public CreateProduct UnitPrice(string unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var sku = Context.GetParamValue(_skuParamAlias!);
        var result = Driver.CreateProduct(sku, _unitPrice);
        
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
            
        return new CommandResult<object, VoidVerification>(
            objectResult, 
            Context, 
            (_, ctx) => new VoidVerification(null, ctx));
    }
}
