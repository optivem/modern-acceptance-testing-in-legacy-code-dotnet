using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Optivem.Commons.Util;
using Optivem.Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class GoToErp : BaseErpCommand<VoidValue, VoidVerification<UseCaseContext>>
{
    public GoToErp(ErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ErpUseCaseResult<VoidValue, VoidVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToErp();
        return new ErpUseCaseResult<VoidValue, VoidVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidVerification<UseCaseContext>(response, ctx));
    }
}
