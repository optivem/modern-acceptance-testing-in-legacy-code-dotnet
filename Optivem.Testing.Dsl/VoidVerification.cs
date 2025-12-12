using Optivem.Results;

namespace Optivem.Testing.Dsl;

public class VoidVerification : BaseSuccessVerification<VoidValue>
{
    private VoidVerification(VoidValue response, Context context) 
        : base(response, context)
    {
    }

    public VoidVerification(Context context)
        : this(VoidValue.Empty, context)
    {
    }
}
