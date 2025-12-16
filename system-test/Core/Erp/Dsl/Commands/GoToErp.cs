using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class GoToErp : BaseErpCommand<VoidValue, VoidResponseVerification<UseCaseContext>>
{
    public GoToErp(ErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override ErpUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>> Execute()
    {
        var result = _driver.GoToErp();
        return new ErpUseCaseResult<VoidValue, VoidResponseVerification<UseCaseContext>>(
            result, 
            _context, 
            (response, ctx) => new VoidResponseVerification<UseCaseContext>(response, ctx));
    }
}
