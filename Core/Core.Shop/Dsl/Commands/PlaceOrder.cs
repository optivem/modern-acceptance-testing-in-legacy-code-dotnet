using Optivem.EShop.SystemTest.Core.Shop.Driver;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Requests;
using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands;

public class PlaceOrder : BaseShopCommand<PlaceOrderResponse, PlaceOrderVerification>
{
    private const string DEFAULT_SKU = "DEFAULT_SKU";
    private const int DEFAULT_QUANTITY = 1;
    private const string DEFAULT_COUNTRY = "US";

    private string? _orderNumberResultAlias;
    private string? _skuParamAlias;
    private string? _quantity;
    private string? _country;

    public PlaceOrder(IShopDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
        Sku(DEFAULT_SKU);
        Quantity(DEFAULT_QUANTITY);
        Country(DEFAULT_COUNTRY);
    }

    public PlaceOrder OrderNumber(string orderNumberResultAlias)
    {
        _orderNumberResultAlias = orderNumberResultAlias;
        return this;
    }

    public PlaceOrder Sku(string skuParamAlias)
    {
        _skuParamAlias = skuParamAlias;
        return this;
    }

    public PlaceOrder Quantity(string quantity)
    {
        _quantity = quantity;
        return this;
    }

    public PlaceOrder Quantity(int quantity)
    {
        return Quantity(quantity.ToString());
    }

    public PlaceOrder Country(string country)
    {
        _country = country;
        return this;
    }

    public override ShopUseCaseResult<PlaceOrderResponse, PlaceOrderVerification> Execute()
    {
        var sku = _context.GetParamValue(_skuParamAlias!);

        var request = new PlaceOrderRequest
        {
            Sku = sku,
            Quantity = _quantity!,
            Country = _country!
        };

        var result = _driver.PlaceOrder(request);

        if (result.IsSuccess && _orderNumberResultAlias != null)
        {
            var orderNumber = result.Value.OrderNumber;
            _context.SetResultEntry(_orderNumberResultAlias, orderNumber);
        }

        return new ShopUseCaseResult<PlaceOrderResponse, PlaceOrderVerification>(
            result, 
            _context, 
            (response, ctx) => new PlaceOrderVerification(response, ctx));
    }
}
