using Optivem.Lang;

namespace Optivem.Testing.Dsl;

public class VoidVerification<TContext> : ResponseVerification<VoidValue, TContext>
{
    public VoidVerification(VoidValue response, TContext context) 
        : base(response, context)
    {
    }
}
