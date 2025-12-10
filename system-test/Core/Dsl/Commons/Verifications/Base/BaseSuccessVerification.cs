using Optivem.EShop.SystemTest.Core.Dsl.Commons;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;

public abstract class BaseSuccessVerification<TResponse>
{
    protected readonly TResponse Response;
    protected readonly Context Context;

    protected BaseSuccessVerification(TResponse response, Context context)
    {
        Response = response;
        Context = context;
    }
}
