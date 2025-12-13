using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Optivem.Lang;
using Optivem.Results;
using Optivem.Testing.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class GoToErp : BaseErpCommand<VoidValue, VoidVerification>
{
    public GoToErp(ErpDriver driver, Context context) 
        : base(driver, context)
    {
    }

    public override CommandResult<VoidValue, VoidVerification> Execute()
    {
        var result = _driver.GoToErp();
        return new CommandResult<VoidValue, VoidVerification>(result, _context, (_, ctx) => new VoidVerification(ctx));
    }
}
