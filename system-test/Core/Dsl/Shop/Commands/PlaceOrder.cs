using Optivem.EShop.SystemTest.Core.Drivers.System;
using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Commands;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands.Base;
using Optivem.EShop.SystemTest.Core.Dsl.Shop.Verifications;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Commands;

public class PlaceOrder : BaseShopCommand<PlaceOrderResponse, PlaceOrderVerification>
{
    private string? _orderNumberResultAlias;
    private string? _skuParamAlias;
    private string _quantity = "1";
    private string _country = "US";

    public PlaceOrder(IShopDriver driver, Context context) 
        : base(driver, context)
    {
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

    public PlaceOrder Country(string country)
    {
        _country = country;
        return this;
    }

    public override CommandResult<PlaceOrderResponse, PlaceOrderVerification> Execute()
    {
        var sku = Context.GetParamValue(_skuParamAlias!);
        var result = Driver.PlaceOrder(sku, _quantity, _country);

        if (result.Success && _orderNumberResultAlias != null)
        {
            var orderNumber = result.GetValue().OrderNumber;
            Context.SetResultEntry(_orderNumberResultAlias, orderNumber);
        }

        return new CommandResult<PlaceOrderResponse, PlaceOrderVerification>(
            result, 
            Context, 
            (response, ctx) => new PlaceOrderVerification(response, ctx));
    }
}
