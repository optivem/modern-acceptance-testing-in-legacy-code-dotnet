using Optivem.EShop.SystemTest.Core.Erp.Driver;
using Optivem.EShop.SystemTest.Core.Erp.Driver.Dtos.Error;
using Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands.Base;
using Commons.Util;
using Commons.Dsl;

namespace Optivem.EShop.SystemTest.Core.Erp.Dsl.Commands;

public class GoToErp : BaseErpCommand<VoidValue, VoidVerification>
{
    public GoToErp(IErpDriver driver, UseCaseContext context) 
        : base(driver, context)
    {
    }

    public override async Task<ErpUseCaseResult<VoidValue, VoidVerification>> Execute()
    {
        var result = await _driver.GoToErp();

        return new ErpUseCaseResult<VoidValue, VoidVerification>(
            result, 
            _context,
            (response, ctx) => new VoidVerification(response, ctx));
    }
}
