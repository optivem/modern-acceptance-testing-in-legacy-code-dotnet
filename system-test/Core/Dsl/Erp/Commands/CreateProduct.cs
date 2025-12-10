using System.Globalization;
using Optivem.EShop.SystemTest.Core.Drivers.External.Erp.Api;
using Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands.Base;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Dsl.Erp.Commands;

public class CreateProduct : BaseErpCommand<object, VoidVerification>
{
    private const decimal DEFAULT_UNIT_PRICE = 20.00m;

    private string? _skuParamAlias;
    private string? _unitPrice;

    public CreateProduct(ErpApiDriver driver, Context context) 
        : base(driver, context)
    {
        UnitPrice(DEFAULT_UNIT_PRICE);
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

    public CreateProduct UnitPrice(decimal unitPrice)
    {
        return UnitPrice(unitPrice.ToString(CultureInfo.InvariantCulture));
    }

    public override CommandResult<object, VoidVerification> Execute()
    {
        var sku = Context.GetParamValue(_skuParamAlias!);
        var result = Driver.CreateProduct(sku, _unitPrice!);
        
        var objectResult = result.Success 
            ? Results.Result<object>.SuccessResult(new object()) 
            : Results.Result<object>.FailureResult(result.GetErrors());
            
        return new CommandResult<object, VoidVerification>(
            objectResult, 
            Context, 
            (_, ctx) => new VoidVerification(null, ctx));
    }
}
