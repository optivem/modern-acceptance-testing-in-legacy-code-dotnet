using Dsl.Gherkin.When;
using Commons.Dsl;
using Commons.Util;
using Optivem.EShop.SystemTest.Core;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Commands.Base;
using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.Testing;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Dsl.Gherkin.Builders.When.PlaceOrder;

public class PlaceOrderBuilder : BaseWhenBuilder<PlaceOrderResponse, PlaceOrderVerification>
{
    private string? _orderNumber;
    private string? _sku;
    private string? _quantity;
    private string? _country;
    private string? _couponCode;

    public PlaceOrderBuilder(SystemDsl app, ScenarioDsl scenario) : base(app, scenario)
    {
        WithOrderNumber(DefaultOrderNumber);
        WithSku(DefaultSku);
        WithQuantity(DefaultQuantity);
        WithCountry(DefaultCountry);
        WithCouponCode(Empty);
    }

    public PlaceOrderBuilder WithOrderNumber(string? orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    public PlaceOrderBuilder WithSku(string? sku)
    {
        _sku = sku;
        return this;
    }

    public PlaceOrderBuilder WithQuantity(string? quantity)
    {
        _quantity = quantity;
        return this;
    }

    public PlaceOrderBuilder WithQuantity(int quantity)
    {
        return WithQuantity(quantity.ToString());
    }

    public PlaceOrderBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public PlaceOrderBuilder WithCouponCode(string? couponCode)
    {
        _couponCode = couponCode;
        return this;
    }

    public PlaceOrderBuilder WithCouponCode()
    {
        return WithCouponCode(DefaultCouponCode);
    }

    protected override ExecutionResult<PlaceOrderResponse, PlaceOrderVerification> Execute(SystemDsl app)
    {
        var result = app.Shop(Channel).PlaceOrder()
            .OrderNumber(_orderNumber)
            .Sku(_sku)
            .Quantity(_quantity)
            .Country(_country)
            .CouponCode(_couponCode)
            .Execute();

        return new ExecutionResultBuilder<PlaceOrderResponse, PlaceOrderVerification>(result)
            .OrderNumber(_orderNumber)
            .CouponCode(_couponCode)
            .Build();
    }
}