using Dsl.Gherkin.Given;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Given;

public class GivenOrderBuilder : BaseGivenBuilder
{
    private string? _orderNumber;
    private string? _sku;
    private string? _quantity;
    private string? _country;
    private string? _couponCodeAlias;
    private OrderStatus _status;

    public GivenOrderBuilder(GivenClause givenClause) : base(givenClause)
    {
        WithOrderNumber(DefaultOrderNumber);
        WithSku(DefaultSku);
        WithQuantity(DefaultQuantity);
        WithCountry(DefaultCountry);
        WithCouponCode(Empty);
        WithStatus(DefaultOrderStatus);
    }

    public GivenOrderBuilder WithOrderNumber(string orderNumber)
    {
        _orderNumber = orderNumber;
        return this;
    }

    public GivenOrderBuilder WithSku(string sku)
    {
        _sku = sku;
        return this;
    }

    public GivenOrderBuilder WithQuantity(string quantity)
    {
        _quantity = quantity;
        return this;
    }

    public GivenOrderBuilder WithQuantity(int quantity)
    {
        return WithQuantity(quantity.ToString());    }

    public GivenOrderBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public GivenOrderBuilder WithCouponCode(string couponCodeAlias)
    {
        _couponCodeAlias = couponCodeAlias;
        return this;
    }

    public GivenOrderBuilder WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    internal override async Task Execute(SystemDsl app)
    {
        (await (await app.Shop(Channel)).PlaceOrder()
            .OrderNumber(_orderNumber)
            .Sku(_sku)
            .Quantity(_quantity)
            .Country(_country)
            .CouponCode(_couponCodeAlias)
            .Execute())
            .ShouldSucceed();

        if (_status == OrderStatus.Cancelled)
        {
            (await (await app.Shop(Channel)).CancelOrder()
                .OrderNumber(_orderNumber)
                .Execute())
                .ShouldSucceed();
        }
    }
}