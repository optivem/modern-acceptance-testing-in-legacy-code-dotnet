namespace Optivem.Commons.Dsl;

public class ResponseVerification<TResponse>
{
    protected readonly TResponse Response;
    protected readonly UseCaseContext Context;

    public ResponseVerification(TResponse response, UseCaseContext context)
    {
        Response = response;
        Context = context;
    }

    public TResponse GetResponse() => Response;
    public UseCaseContext GetContext() => Context;
}
