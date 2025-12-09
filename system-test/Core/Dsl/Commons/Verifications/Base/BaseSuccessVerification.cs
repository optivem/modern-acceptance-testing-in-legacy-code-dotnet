using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;

public abstract class BaseSuccessVerification<TResponse>
{
    protected readonly TResponse Response;
    protected readonly TestContext Context;

    protected BaseSuccessVerification(TResponse response, TestContext context)
    {
        Response = response;
        Context = context;
    }
}
