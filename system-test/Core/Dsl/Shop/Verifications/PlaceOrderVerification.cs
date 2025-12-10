using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.Testing.Dsl;
using Shouldly;

namespace Optivem.EShop.SystemTest.Core.Dsl.Shop.Verifications;

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
        Response.OrderNumber.ShouldStartWith(prefix);
        return this;
    }
}
