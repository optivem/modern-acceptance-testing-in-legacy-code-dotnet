namespace Optivem.Testing.Dsl;

public class ResponseVerification<TResponse, TContext>
{
    protected readonly TResponse Response;
    protected readonly TContext Context;

    public ResponseVerification(TResponse response, TContext context)
    {
        Response = response;
        Context = context;
    }

    public TResponse GetResponse() => Response;
    public TContext GetContext() => Context;
}
