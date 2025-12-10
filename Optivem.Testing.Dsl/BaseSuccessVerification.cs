namespace Optivem.Testing.Dsl;

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
