using Optivem.EShop.SystemTest.Core.Dsl.Commons;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;

public class VoidVerification : BaseSuccessVerification<object>
{
    public VoidVerification(object response, Context context) 
        : base(response, context)
    {
    }
}
