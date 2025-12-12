using Optivem.EShop.SystemTest.Core.Shop.Driver.Dtos.Responses;
using Optivem.Testing.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;

public class PlaceOrderVerification : BaseSuccessVerification<PlaceOrderResponse>
{
    public PlaceOrderVerification(PlaceOrderResponse response, Context context) 
        : base(response, context)
    {
    }

    public PlaceOrderVerification OrderNumber(string orderNumberResultAlias)
    {
        var expectedOrderNumber = Context.GetResultValue(orderNumberResultAlias);
        var actualOrderNumber = Response.OrderNumber;
        
        actualOrderNumber.ShouldBe(expectedOrderNumber, $"Expected order number to be '{expectedOrderNumber}', but was '{actualOrderNumber}'");
        
        return this;
    }

    public PlaceOrderVerification OrderNumberStartsWith(string prefix)
    {
        Response.OrderNumber.ShouldStartWith(prefix, Case.Sensitive, $"Expected order number to start with '{prefix}', but was '{Response.OrderNumber}'");
        return this;
    }
}
