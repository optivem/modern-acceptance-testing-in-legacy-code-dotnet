using Optivem.EShop.SystemTest.Core.Drivers.System.Commons.Dtos;
using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;
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
        Response.OrderNumber.ShouldBe(expectedOrderNumber, 
            $"Expected order number to be '{expectedOrderNumber}', but was '{Response.OrderNumber}'");
        return this;
    }
}
