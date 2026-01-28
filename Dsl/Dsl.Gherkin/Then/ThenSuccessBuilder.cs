using Optivem.EShop.SystemTest.Core.Shop.Dsl.Verifications;
using Optivem.EShop.SystemTest.Core.Shop.Commons.Dtos.Orders;
using static Optivem.EShop.SystemTest.Core.Gherkin.GherkinDefaults;
using Dsl.Gherkin.Then;
using Optivem.Commons.Dsl;
using Optivem.Commons.Util;

namespace Optivem.EShop.SystemTest.Core.Gherkin.Then;

public class ThenSuccessBuilder<TSuccessResponse, TSuccessVerification> : BaseThenBuilder<TSuccessResponse, TSuccessVerification> 
    where TSuccessVerification : ResponseVerification<TSuccessResponse>
{
    private readonly TSuccessVerification _successVerification;

    public ThenSuccessBuilder(ThenClause<TSuccessResponse, TSuccessVerification> thenClause, TSuccessVerification successVerification) : base(thenClause)
    {
        _successVerification = successVerification;
    }

    public ThenSuccessBuilder<TSuccessResponse, TSuccessVerification> HasOrderNumberPrefix(string prefix)
    {
        switch (_successVerification)
        {
            case PlaceOrderVerification placeOrderVerification:
                placeOrderVerification.OrderNumberStartsWith(prefix);
                break;
            case ViewOrderVerification viewOrderVerification:
                viewOrderVerification.OrderNumberHasPrefix(prefix);
                break;
            default:
                throw new InvalidOperationException("hasOrderNumberPrefix is not supported for this verification type");
        }

        return this;
    }
}