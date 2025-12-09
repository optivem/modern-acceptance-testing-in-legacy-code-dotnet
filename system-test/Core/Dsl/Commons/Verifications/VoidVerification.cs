using Optivem.EShop.SystemTest.Core.Dsl.Commons.Context;
using Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications.Base;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons.Verifications;

public class VoidVerification : BaseSuccessVerification<object>
{
    public VoidVerification(object response, TestContext context) 
        : base(response, context)
    {
    }
}
