using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Client.Dtos.Requests;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;
using System.CodeDom;
using System.Globalization;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class CreateProduct : BaseErpCommand<VoidValue, VoidResponseVerification<UseCaseContext>>
{
    private const string DEFAULT_SKU = "DEFAULT_SKU";
    private const decimal DEFAULT_UNIT_PRICE = 20.00m;
    private const string DEFAULT_TITLE = "Test Product Title";
    private const string DEFAULT_DESCRIPTION = "Test Product Description";
    private const string DEFAULT_CATEGORY = "Test Category";
    private const string DEFAULT_BRAND = "Test Brand";

    private string? _skuParamAlias;
    private string? _unitPrice;
    private string? _title;
    private string? _description;
    private string? _category;
    private string? _brand;

    public CreateProduct(ErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
        Sku(DEFAULT_SKU)
        .UnitPrice(DEFAULT_UNIT_PRICE)
        .Title(DEFAULT_TITLE)
        .Description(DEFAULT_DESCRIPTION)
        .Category(DEFAULT_CATEGORY)
        .Brand(DEFAULT_BRAND);
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

    public CreateProduct Title(string title)
    {
        _title = title;
        return this;
    }

    public CreateProduct Description(string description)
    {
        _description = description;
        return this;
    }

    public CreateProduct Category(string category)
    {
        _category = category;
        return this;
    }

    public CreateProduct Brand(string brand)
    {
        _brand = brand;
        return this;
    }

    public override ErpUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>> Execute()
    {
        var sku = _context.GetParamValue(_skuParamAlias!);

        var request = new CreateProductRequest
        {
            Id = sku,
            Title = _title,
            Description = _description,
            Price = _unitPrice,
            Category = _category,
            Brand = _brand
        };

        var result = _driver.CreateProduct(request);
            
        return new ErpUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidResponseVerification<UseCaseContext>(response, ctx));
    }
}
