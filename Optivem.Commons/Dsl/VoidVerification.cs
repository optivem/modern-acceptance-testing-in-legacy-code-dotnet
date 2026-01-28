using Optivem.Commons.Util;

namespace Optivem.Commons.Dsl;

public class VoidVerification : ResponseVerification<VoidValue>
{
    public VoidVerification(VoidValue response, UseCaseContext context) 
        : base(response, context)
    {
    }
}
