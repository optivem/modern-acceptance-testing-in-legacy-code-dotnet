using Optivem.Lang;

namespace Optivem.Testing.Dsl;

public class VoidResponseVerification<TContext> : ResponseVerification<VoidValue, TContext>
{
    public VoidResponseVerification(VoidValue response, TContext context) 
        : base(response, context)
    {
    }
}
